VAR score = 0
VAR total_questions = 5
VAR passing_score =  3
VAR current_question = 1
=== start
Empezaremos con el cuestionario. En total son {total_questions}, <>
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
¿Qué tipo de aves se pueden observar desde el café mirador?
    + [Patos y gansos] -> incorrect
    + [Tucanes, colíbres y águilas] -> correct
    + [Halcones y búhos] -> incorrect
    + [Pinguinos y flamencos] -> incorrect
    
=== q2
Pregunta {current_question}:<>
¿Cuál es una forma efectiva de ayudar a los turistas a disfrutar su visita?
    +[Proporcionar mapas y recomendar lugares.] -> correct
    +[Ofrecer souvenirs y dejarlos explorar.] -> incorrect
    +[Dirigirlos a tiendas y restaurantes.] -> incorrect
    +[Decirles que busquen información online.] -> incorrect

=== q3
Pregunta {current_question}:<>
¿Cuál de los siguientes no es un elemento esencial en un equipo de primeros auxilios?
    + [Tijeras de emergencia] -> incorrect
    + [Guantes desechables] -> incorrect
    + [Termo para bebidas] -> correct
    + [Botiquín] -> incorrect
    

=== q4
Pregunta {current_question}:<>
¿Cuál de las siguientes actividades es una opción para los visitantes en la naturaleza?
    + [Senderismo] -> correct
    + [Ciclismo] -> incorrect
    + [Ver una película] -> incorrect
    + [Hacer una barbacoa en casa] -> incorrect
    
=== q5
Pregunta {current_question}:<>
¿Cuál es un desafío común en la gestión de la estación?
    +[Actividades sin plan.] -> incorrect
    +[Ignorar la coordinación.] -> incorrect
    +[Aumentar personal sin control.] -> incorrect
    +[Equilibrar recursos y necesidades.] -> correct