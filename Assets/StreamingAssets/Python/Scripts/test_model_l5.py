import os
import joblib
import json
import numpy as np
import pandas as pd

def parse_float_comma(val):
    if isinstance(val, str):
        val = val.replace(',', '.')
    try:
        return float(val)
    except (ValueError, TypeError):
        return np.nan  # o un valor por defecto

script_dir = os.path.dirname(__file__)
input_path = os.path.join(script_dir, '..', 'Data', 'input.json')
output_path = os.path.join(script_dir, '..', 'Data', 'output.json')
model_path = os.path.join(script_dir, '..', 'Models', 'model_l5.pkl')
encoder_info_path = os.path.join(script_dir, '..', 'Data', 'encoder_info.json')
encoder_path = os.path.join(script_dir, '..', 'Models', 'color_encoder.pkl')

# Cargar el modelo
model = joblib.load(model_path)

with open(input_path, 'r') as f:
    data = json.load(f)

width = parse_float_comma(data['width'])
height = parse_float_comma(data['height'])
weight = parse_float_comma(data['weight'])
color = data['color']

input_df = pd.DataFrame([{
    'Width': width,
    'Height': height,
    'Weight': weight,
    'Color': color
}])

# Verificar tipo de codificador usado
with open(encoder_info_path, 'r') as f:
    encoder_info = json.load(f)

categorization = encoder_info.get("Color", "")
if categorization == "One-Hot":
    ohe = joblib.load(encoder_path)
    color_encoded = ohe.transform(input_df[['Color']])
    color_df = pd.DataFrame(color_encoded, columns=ohe.get_feature_names_out())
    color_df.index = input_df.index
    input_df = pd.concat([input_df.drop(columns=['Color']), color_df], axis=1)

elif categorization == "Label":
    le = joblib.load(encoder_path)
    input_df['Color'] = le.transform(input_df['Color'].astype(str))

# Asegurarse de que input_df tenga las mismas columnas que el modelo espera
# (esto es útil sobre todo en OneHotEncoder)
model_columns = getattr(model, 'feature_names_in_', None)
if model_columns is not None:
    for col in model_columns:
        if col not in input_df.columns:
            input_df[col] = 0  # Añadir columnas faltantes con 0
    input_df = input_df[model_columns]  # Reordenar columnas

prediction = model.predict(input_df)[0]

output_data = {
    'width': width,
    'height': height,
    'weight': weight,
    'color': color,
    'predicted_animal': prediction
}

with open(output_path, 'w') as f:
    json.dump(output_data, f, indent=4)