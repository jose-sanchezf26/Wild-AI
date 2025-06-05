import pandas as pd
import os
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_absolute_error, r2_score
import joblib

script_dir = os.path.dirname(__file__)

csv_path = os.path.join(script_dir, '..', 'Data', 'animal_data.csv')

data = pd.read_csv(csv_path)
print(data.head())
X = data[["Width", "Height"]]
y = data["Weight"]

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Creamos el modelo de regresión lineal
model = LinearRegression()

# Entrenamos el modelo
model.fit(X_train, y_train)

model_save_path = os.path.join(script_dir, '..', 'Models', 'model_l1.pkl')

joblib.dump(model, model_save_path)