import pandas as pd
import os
import json
import seaborn as sns
import numpy as np
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
from sklearn.pipeline import Pipeline
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.impute import SimpleImputer
from sklearn.metrics import mean_absolute_error, r2_score, confusion_matrix, ConfusionMatrixDisplay, classification_report
from imblearn.over_sampling import SMOTE
from sklearn.utils import resample
from sklearn.preprocessing import OneHotEncoder, LabelEncoder, MinMaxScaler, StandardScaler, RobustScaler, Normalizer
import joblib

def parse_val(val, default=1.0):
    if val in [None, ""]:
        return default
    try:
        return float(val)
    except ValueError:
        return default



script_dir = os.path.dirname(__file__)
# TODO CAMBIAR EL NOMBRE DEL CSV POR EL BUENOO
csv_path = os.path.join(script_dir, '..', 'Data', 'animal_data.csv')
params_path = os.path.join(script_dir, '..', 'Data', 'model_parameters.json')

# Cargamos los parámetros del modelo
with open(params_path, 'r', encoding="utf-8") as file:
    params = json.load(file)

# Cargamos los datos
data = pd.read_csv(csv_path)

# Preprocesamiento
# Separar variables categóricas
target_col = 'Animal'
categorical_cols = [col for col in data.select_dtypes(include=['object', 'category']).columns if col != target_col]
numerical_cols = data.select_dtypes(include=['int64', 'float64']).columns.tolist()


# Tratamiento de valores nulos
null_strategy = params.get("Datos nulos", "")
if null_strategy == "Eliminar":
    data = data.dropna()
elif null_strategy == "Imputar con constante":
    data = data.fillna(0)  
elif null_strategy in ["Media", "Mediana", "Moda"]:
    imputer_strategy = {
        "Media": "mean",
        "Mediana": "median",
        "Moda": "most_frequent"
    }
    imputer = SimpleImputer(strategy=imputer_strategy[null_strategy])
    data[numerical_cols] = imputer.fit_transform(data[numerical_cols])
    for col in categorical_cols:
        if data[col].isnull().any():
            data[col].fillna(data[col].mode()[0])

# Categorización de variables
categorization = params.get("Categorización", "")
encoder_info = {"Color": categorization}
with open(os.path.join(script_dir, '..', 'Data', 'encoder_info.json'), 'w') as f:
    json.dump(encoder_info, f)

if categorization == "One-Hot":
    ohe = OneHotEncoder(handle_unknown='ignore', sparse_output=False)
    color_encoded = ohe.fit_transform(data[['Color']])  # Esto ya es un array denso
    color_df = pd.DataFrame(color_encoded, columns=ohe.get_feature_names_out())
    color_df.index = data.index
    data = pd.concat([data.drop(columns=['Color']), color_df], axis=1)
    print(data.head())
    joblib.dump(ohe, os.path.join(script_dir, '..', 'Models', 'color_encoder.pkl'))
elif categorization == "Label":
    le = LabelEncoder()
    for col in categorical_cols:
        data[col] = le.fit_transform(data[col].astype(str))
    joblib.dump(le, os.path.join(script_dir, '..', 'Models', 'color_encoder.pkl'))
    

# Normalización
scaling_method = params.get("Nomalización", "")
if scaling_method == "Min-Max":
    scaler = MinMaxScaler()
elif scaling_method == "Z-Score":
    scaler = StandardScaler()
elif scaling_method == "Robust":
    scaler = RobustScaler()
elif scaling_method == "L1/L2":
    scaler = Normalizer(norm='l2')
elif scaling_method == "LogTransform":
    data[numerical_cols] = data[numerical_cols].apply(lambda x: np.log1p(x))
    scaler = None

# Separación de x e y
data = data.dropna(subset=['Animal'])  # Eliminamos filas con valores nulos
X = data.drop(columns=['Animal'])
y = data['Animal'].astype(str)  # Aseguramos que y sea de tipo string

# Dividimos los datos en entrenamiento y prueba
train_percent = parse_val(params.get("% Train", 80))
test_percent = 100 - train_percent
test_size = test_percent / 100.0
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=test_size, random_state=42, stratify=y)

# Técnica de desbalanceo
imbalance = params.get("Técnica de desbalanceo", "")
if imbalance == "Submuestreo":
    data_balanced = pd.concat([X, y], axis=1)
    min_size = data_balanced[target_col].value_counts().min()
    data_list = [data_balanced[data_balanced[target_col] == cls].sample(min_size) for cls in data_balanced[target_col].unique()]
    data = pd.concat(data_list)
    X = data.drop(columns=[target_col])
    y = data[target_col]
elif imbalance == "Sobremuestreo":
    data_balanced = pd.concat([X, y], axis=1)
    max_size = data_balanced[target_col].value_counts().max()
    data_list = [resample(data_balanced[data_balanced[target_col] == cls],
                        replace=True,
                        n_samples=max_size,
                        random_state=42) for cls in data_balanced[target_col].unique()]
    data = pd.concat(data_list)
    X = data.drop(columns=[target_col])
    y = data[target_col]
elif imbalance == "SMOTE":
    sm = SMOTE(random_state=42)
    X, y = sm.fit_resample(X, y)


# Creamos el modelo
model = None
model_type = params.get("modelType")
print(f"Creando modelo de tipo: {model_type}")
if model_type == "Logistic Regression":
    c = parse_val(params.get("C"), default=1.0)
    penalty = params.get("penalty", "l2")
    max_iter = int(parse_val(params.get("max_iter"), default=100))
    model = LogisticRegression(C=c, penalty=penalty, max_iter=max_iter, solver="saga")

elif model_type == "Decision Tree":
    max_depth = int(parse_val(params.get("max_depth"), default=None))
    min_samples_split = int(parse_val(params.get("min_samples_split"), default=2))
    criterion = params.get("criterion", "gini")
    model = DecisionTreeClassifier(max_depth=max_depth, min_samples_split=min_samples_split, criterion=criterion)

else:
    raise ValueError(f"Modelo no soportado: {model_type}")



# Entrenamos el modelo y lo guardamos
model.fit(X_train, y_train)
model_save_path = os.path.join(script_dir, '..', 'Models', 'model_l5.pkl')

joblib.dump(model, model_save_path)

# Parte de generación de métricas y gráficas
# Evaluación
y_pred = model.predict(X_test)


# Métricas
report = classification_report(y_test, y_pred, digits=2)
# Definimos ruta y guardamos
metrics_txt_path = os.path.join(script_dir, '..', 'Data', 'classification_report.txt')
with open(metrics_txt_path, 'w', encoding='utf-8') as f:
    f.write(report)

# Configuración para gráficas
background_color = '#002F00'
text_color = '#00EE00'
titlefontsize = 30
fontsize = 25
original_image_size = (6.7,4)
factor = 2.5
image_size = original_image_size[0] * factor, original_image_size[1] * factor

# Matriz de confusión
cm = confusion_matrix(y_test, y_pred, labels=model.classes_)
# Crear la figura
plt.figure(figsize=(10, 10))
ax = sns.heatmap(cm, annot=True, fmt='d', cmap='Greens', 
                 xticklabels=model.classes_, 
                 yticklabels=model.classes_,
                 cbar=False, annot_kws={"size": fontsize})

# Estilo
plt.title("Matriz de Confusión", fontsize=titlefontsize, color=text_color, pad=20)
plt.xlabel("Predicción", fontsize=fontsize, color=text_color)
plt.ylabel("Real", fontsize=fontsize, color=text_color)
plt.xticks(fontsize=fontsize, color=text_color)
plt.yticks(fontsize=fontsize, color=text_color)
plt.gca().set_facecolor(background_color)
plt.gcf().patch.set_facecolor(background_color)

# Guardar imagen
output_img_path = os.path.join(script_dir, '..', 'Images', 'confusion_matrix.png')
plt.tight_layout()
plt.savefig(output_img_path, facecolor=background_color)
plt.close()

from sklearn.model_selection import learning_curve

# CURVA DE APRENDIZAJE
train_sizes, train_scores, test_scores = learning_curve(
    model, X, y, cv=5, scoring='accuracy', n_jobs=-1, train_sizes=np.linspace(0.1, 1.0, 10))

train_scores_mean = np.mean(train_scores, axis=1)
test_scores_mean = np.mean(test_scores, axis=1)

plt.figure(figsize=image_size)
plt.plot(train_sizes, train_scores_mean, label="Entrenamiento", color=text_color, lw=3)
plt.plot(train_sizes, test_scores_mean, label="Validación", color="#E377C2", lw=3)
plt.title("Curva de Aprendizaje", fontsize=titlefontsize, color=text_color)
plt.xlabel("Tamaño del conjunto de entrenamiento", fontsize=fontsize, color=text_color)
plt.ylabel("Precisión", fontsize=fontsize, color=text_color)
plt.legend(loc="best", fontsize=fontsize)
plt.grid(True)
plt.gca().set_facecolor(background_color)
plt.gcf().patch.set_facecolor(background_color)

# Guardar imagen
lc_path = os.path.join(script_dir, '..', 'Images', 'learning_curve.png')
plt.tight_layout()
plt.savefig(lc_path, facecolor=background_color)
plt.close()
