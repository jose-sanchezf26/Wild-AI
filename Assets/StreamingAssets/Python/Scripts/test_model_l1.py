import os
import joblib
import json
import numpy as np
import pandas as pd

script_dir = os.path.dirname(__file__)
input_path = os.path.join(script_dir, '..', 'Data', 'input.json')
output_path = os.path.join(script_dir, '..', 'Data', 'output.json')
model_path = os.path.join(script_dir, '..', 'Models', 'model_l1.pkl')

# Cargar el modelo
model = joblib.load(model_path)

with open(input_path, 'r') as f:
    data = json.load(f)

width = data['width']
height = data['height']

input_df = pd.DataFrame([{
    'Width': width,
    'Height': height
}])
prediction = model.predict(input_df)[0]

output_data = {
    'width': width,
    'height': height,
    'predicted_weight': round(float(prediction), 2)
}

with open(output_path, 'w') as f:
    json.dump(output_data, f, indent=4)