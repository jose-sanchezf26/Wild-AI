# Eventos de Wild AI

| User  | Game ID  | Session ID | Event Type | Time  | Data  | Descripción  |
|-------|-------------|------------------------------------|------------|---------------------|-------|---------------|
| Jose  | Wild AI | 13072935-16e3-4483-8f38-73d4e3ecf693 | wai-log_in  | 17/02/2025 17:01:21 | {}    | Inicia sesión |
| Jose  | Wild AI | 13072935-16e3-4483-8f38-73d4e3ecf693 | wai-log_out | 17/02/2025 17:18:20 | {}    | Cierra sesión |
| Jose  | Wild AI | 13072935-16e3-4483-8f38-73d4e3ecf693 | wai-start_level | 17/02/2025 17:18:20 | {"level": 1} | Empieza un nivel |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-exit_level | 03/03/2025 12:30:58         | {"level": 1}           | Sale de un nivel |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-complete_level | 03/03/2025 12:30:58         | {"level": 1}           | Completa los objetivos de un nivel |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-complete_objective | 03/03/2025 12:30:58         | {"level": 1, "objective": "Registra cinco animales"}           | Completa un objetivo de un nivel |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-open_window | 03/03/2025 12:30:58         | {"level": 1, "window": "Register"}           | Abre una ventana |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-close_window | 03/03/2025 12:30:58         | {"level": 1, "window": "Register"}           | Cierra una ventana |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-go_train_scene | 03/03/2025 12:30:58         | {"level": 5}           | Abre la escena de train |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-go_test_scene | 03/03/2025 12:30:58         | {"level": 5}           | Abre la escena de test |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-register_animal | 03/03/2025 12:30:58         | {"level": 1, "type": "bear", "height": 120, "width": 90, "weight": 600, "color": "brown"}           | Registra un animal manualmente |
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-delete_animal | 03/03/2025 12:30:58         | {"level": 1, "type": "bear", "height": 120, "width": 90, "weight": 600, "color": "brown"}           | Borra un animal manualmente|
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-predict_animal | 03/03/2025 12:30:58         |     {"height": "1,08","width": "1,68","weight": "128,47","color": "Marron","predictedType": "Zorro","level": 5}      | Prueba el modelo con un animal|
| Jose  | Wild AI | 2d2bb501-78f8-4dd9-99a1-ad80160f2792      | wai-model_created | 03/03/2025 12:30:58         | {"parameters": {"modelType": "Decision Tree","C": "","penalty": "l1","max_iter":"","max_depth": "5","min_samples_split": "5","criterion": "gin "% Train": "50""% Test": "","Datos nulos": "","Normalización": "","Categorización": "One-Hot","Técnica de desbalanceo": ""    }}      |Crea un modelo sin errores|