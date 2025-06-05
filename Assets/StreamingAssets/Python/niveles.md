| Nivel  | Título   | Contenido nuevo | Mecánicas nuevas| Objetivo |
|------------|------------|------------|-----------------|--------|
| 1| Introducción al Machine Learning | | 1. Registro de datos: ancho y alto mediante la regla y peso con la báscula.  <br> 2. Visualización de los datos datos mediante una tabla. <br> 3. Test del modelo situando al jugador en un escenario de prueba donde pueda medir el ancho, el alto y el peso de un animal y compararlo con la predicción del modelo | Introducir al jugador en Machine Learning.
| 2 | Limpieza de datos| Se añaden animales con datos nulos o ruidosos en sus atributos y se explica el primer método de ML, la regresión lineal. | 1. Elección de imputar o descartar los datos nulos detectados. <br> 2. Eleción de corregir o eliminar los datos ruidosos. <br> 3. Elección de atributos en el modelo de regresión lineal.| Destacar la importancia de tener unos datos limpios a la hora de entrenar un modelo y explicar la regresión lineal.
| 3 | Preprocesamiento general | Normalización, codificación de categorías y elección de variable objetivo (probelma de regresión)| 1. Elección de la técnica de normalización. <br> 2. Elección de codificación. <br> Elección de la variable objetivo entre peso, alto y ancho. | Explicar el proceso general del preprocesamiento.
| 4 | Métricas en regresión | Se añade una ventana en la escena de test donde puede generar datos de test para visualizar gráficas de métricas, el error en el entrenamiento y el test y comprobar si tiene sobreajuste o no. Se introduce un nuevo método de regresión. | 1. Generar datos de test. <br> 2. Explicación y visualización de métricas. <br> Elección de la variable objetivo entre peso, alto y ancho. | Explicar cómo las métricas pueden determinar la calidad de tu modelo, y la importancia del factor de generalización del mismo.
| Z (Nivel que se va a crear para el TFM) | Desbalanceo de clases | Se añade una ventana en la escena en el preprocesamiento donde permite ver el número de instancias de cada clase del animal. Se añaden distintas opciones para el desbalanceo de clases. | 1. Elección de técnica de desbalanceo | Dar a entender la importancia de tener ejemplos de todos los tipos de animales para crear un modelo de clasificación consistente.


El nivel a incluir para el TFM va a tener todas las mecánicas de los niveles anteriores que aun no habrán sido creados, de esta forma se simplificaría la creación de los niveles previos omitiendo información y adaptando las ventanas de ayuda con sus respectivas explicaciones.
El nivel va a incluir:
- Cada vez que se registra un animal de forma manual, se añaden X más del mismo tipo, además se intentará implementar un nuevo objeto que se encuentre por el mapa que simule una "nota" con muchos datos, de forma que se añadan a los datos del jugador (será lo último ya que no es esencial).
- En el preprocesamiento, opciones para el tratamiento de datos nulos y ruidosos, normalización de datos, categorización de variables de texto y una nueva ventana donde pueda visualizar distintas gráficas relacionadas con los datos (gráfica de desbalanceo, las demás todavía por determinar).
- En la parte de la creación de modelo, la opción de modificar los parámetros del algoritmo de clasificación (Random Forest) y explicación del mismo (estoy dudando de si hacer yo la explicación o proporcionar un enlace a algún vídeo).
- En el escenario de test se podrá probar el modelo de forma individual por animal, al igual que en el primer nivel, pero habrá una nueva ventana donde el jugador pueda generar datos de test y ver distintas gráficas relacionadas con el desbalanceo, como la matriz de confusión, También podrá ver distintas métricas como el f1-score, recall y accuracy.
- Los objetivos son:
    - Registra en la base de datos los cuatro tipos de animales presenten en el escenario.
    - Prueba el modelo con todas las opciones de desbalanceo.
    - Consigue un modelo con un valor para la métrica X de como mínimo Y. 


