VAR score = 0
VAR total_questions = 3
VAR passing_score =  2
VAR current_question = 1
=== start
!Bienvenido al cuestionario del biomonitor! <>
Estas a punto de responder algunas preguntas.

¿Estás listo?
* [Sí]
    -> questionnaire
* [No]
    Vale avisame cuando quieras realizar el cuestionario.
    -> END

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
        - else: ->finish_test
    }

=== q1
Pregunta 1:<>
¿Cuál es el planeta más cercano al sol?
    + [Mercurio] -> correct
    + [Venus] -> incorrect
    + [Marte] -> incorrect
    
=== q2
Pregunta 2:<>
¿Qué gas respiramos mayormente en la tierra?
    +[Oxígeno] -> incorrect
    +[Hidrogeno] -> incorrect
    +[Nitrógeno] -> correct
    +[Dióxido de carbono] -> incorrect

=== q3
Pregunta 3:<>
¿Qué organo es responsable de bombear la sangre en el cuerpo humano?
    + [El cerebro] -> incorrect
    + [El corazón] -> correct
    + [Los pulmones] -> incorrect
    + [El estómago] -> incorrect