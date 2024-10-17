VAR score = 0
VAR total_questions = 5
VAR passing_score =  3
VAR current_question = 1
=== start
Empezaremos con el cuestionario. En total son {total_questions} preguntas, <>
y necesitas un mínimo de {passing_score} respuestas correctas para continuar al siguiente módulo.
-> questionnaire

=== correct
    ¡Correcta! ¡Buen trabajo!
    ~ current_question++
    ~ score++
    -> questionnaire


=== incorrect
    Lo siento, esa no es la respuesta correcta.
    ~ current_question ++
    -> questionnaire

=== finish_test
Con esto hemos terminado el cuestionario.
Tu puntaje final es {score} de {total_questions}.
~current_question = 1
->->

=== questionnaire
    {current_question:
        - 1: ->q1
        - 2: ->q2
        - 3: ->q3
        - 4: ->q4
        - 5: ->q5
        - else: ->finish_test
    }

=== q1
Pregunta {current_question}:<>
¿Cuál es la especialidad de la cocina de Luisa?
    + [Ofrecer platos de comida rápida.] -> incorrect
    + [Especializarse en cocina internacional.] -> incorrect
    + [Promover la cocina local de las arrierías.] -> correct
    + [Centrar su menú en postres únicamente.] -> incorrect
    
=== q2
Pregunta {current_question}:<>
¿Con qué equipo trabaja Luisa en la cocina del restaurante?
    +[Un equipo de chefs internacionales] -> incorrect
    +[Un equipo de mujeres de la comunidad local] -> correct
    +[Estudiantes de gastronomía] -> incorrect
    +[Cocineros expertos contratados del extranjero] -> incorrect

=== q3
Pregunta {current_question}:<>
¿Qué prácticas sostenibles implementa Luisa en la cocina?
    + [Usa ingredientes importados] -> incorrect
    + [Usa productos enlatados] -> incorrect
    + [Usa ingredientes frescos y locales] -> correct
    + [Prepara comida rápida para turistas] -> incorrect
    

=== q4
Pregunta {current_question}:<>
¿Qué actividad se ofrece a los visitantes interesados en el avistamiento de aves?
    + [Tours en bicicleta] -> incorrect
    + [Clases de cocina] -> incorrect
    + [Concursos de fotografía] -> incorrect
    + [Sesiones guiadas de avistamiento al amanecer] -> correct
    
=== q5
Pregunta {current_question}:<>
¿Qué tipo de bebidas y alimentos se sirven en el café mirador?
    +[Jugos y ensaladas] -> incorrect
    +[Café local de alta calidad y repostería casera] -> correct
    +[Comidas rápidas y refrescos] -> incorrect
    +[Bebidas alcohólicas y aperitivos] -> incorrect
    