import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
import os


script_dir = os.path.dirname(__file__)

csv_path = os.path.join(script_dir, '..', 'Data', 'animal_data.csv')

df = pd.read_csv(csv_path)


# Colores
background_color = '#002F00'
text_color = '#00EE00'
titlefontsize = 30
fontsize = 25
original_image_size = (6.7,4)
factor = 2.5
image_size = original_image_size[0] * factor, original_image_size[1] * factor

# --- Gráfico 1: Balanceo de clases (Animal) ---
plt.figure(figsize=image_size)
ax1 = df['Animal'].value_counts(dropna=False).plot(kind='bar', color=text_color)
plt.title("Distribución de animales (Balanceo de clases)", color=text_color, fontsize=titlefontsize)
plt.xlabel("Animal", color=text_color, fontsize=fontsize)
plt.ylabel("Frecuencia", color=text_color, fontsize=fontsize)
plt.xticks(rotation=45, color=text_color, fontsize=fontsize)
plt.yticks(color=text_color, fontsize=fontsize)

ax1.set_facecolor(background_color)
fig1 = plt.gcf()
fig1.patch.set_facecolor(background_color)

plt.tight_layout()
plt.savefig(os.path.join(script_dir, '..', 'Images', 'balanced.png'))
plt.close()

# --- Gráfico 2: Nulos por columna ---
plt.figure(figsize=image_size)
ax2 = df.isnull().sum().plot(kind='bar', color=text_color)
plt.title("Cantidad de datos nulos por columna", color=text_color, fontsize=titlefontsize)
plt.xlabel("Columna", color=text_color, fontsize=fontsize)
plt.ylabel("Número de nulos", color=text_color, fontsize=fontsize)
plt.xticks(rotation=45, color=text_color, fontsize=fontsize)
plt.yticks(color=text_color, fontsize=fontsize)

ax2.set_facecolor(background_color)
fig2 = plt.gcf()
fig2.patch.set_facecolor(background_color)
plt.tight_layout()
plt.savefig(os.path.join(script_dir, '..', 'Images', 'null.png'))
plt.close()

# Gráfico 3: distribución de una característica
plt.figure(figsize=image_size)
sns.boxplot(df[['Weight', 'Height', 'Width']], color = text_color)
plt.title("Distribución de características numéricas", color=text_color, fontsize=titlefontsize)
plt.xlabel("Atributo", color=text_color, fontsize=fontsize)
plt.ylabel("Valor", color=text_color, fontsize=fontsize)
plt.xticks(color=text_color, fontsize=fontsize)
plt.yticks(color=text_color, fontsize=fontsize)
ax = plt.gca()
ax.set_facecolor(background_color)
fig = plt.gcf()
fig.patch.set_facecolor(background_color)
plt.tight_layout()
plt.savefig(os.path.join(script_dir, '..', 'Images', 'distribution.png'))
plt.close()

# Gráfico de mapa de correlación
correlation_matrix = df[['Weight', 'Height', 'Width']].corr()
plt.figure(figsize=image_size)
fig = plt.gcf()
fig.patch.set_facecolor(background_color)

ax = sns.heatmap(
    correlation_matrix,
    annot=True,
    cmap="Greens",  # puedes cambiar esto por otra paleta si quieres
    cbar=True,
    fmt=".2f",
    annot_kws={"color": text_color, "fontsize": fontsize}
)

# Estilo
ax.set_facecolor(background_color)
plt.title("Correlación entre características", color=text_color, fontsize=titlefontsize)
plt.xticks(color=text_color, fontsize=fontsize)
plt.yticks(color=text_color, fontsize=fontsize, rotation=0)

# Guardar imagen
plt.tight_layout()
output_path = os.path.join(script_dir, '..', 'Images', 'correlation.png')
plt.savefig(output_path)
plt.close()