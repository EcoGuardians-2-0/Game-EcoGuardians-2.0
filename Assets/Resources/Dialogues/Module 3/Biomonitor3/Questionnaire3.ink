VAR score = 0
VAR total_questions = 6
VAR passing_score =  4
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
        - 6: ->q6
        - else: ->finish_test
    }

=== q1
Pregunta {current_question}:<>
¿Cuál es el impacto positivo que tiene la estación en la comunidad?
    + [Disminuir el acceso a servicios locales.] -> incorrect
    + [Promover el uso de vehículos no sostenibles.] -> incorrect
    + [Generar empleo y fomentar el turismo responsable.] -> correct
    + [Aumentar la competencia entre negocios locales.] -> incorrect
    
=== q2
Pregunta {current_question}:<>
¿Cómo contribuyen los eventos al éxito del programa Awaq Bio-Tech?
    +[Aumentar los costos del programa.] -> incorrect
    +[Formar biomonitores y compartir conocimiento] -> correct
    +[Ofrecer descuentos en productos.] -> incorrect
    +[Limitar el acceso a información.] -> incorrect

=== q3
Pregunta {current_question}:<>
¿Cuáles son las temáticas de los eventos organizados en el Volumen de Conocimiento?
    + [Seminarios sobre marketing y finanzas.] -> incorrect
    + [Talleres de cocina y catas de vino.] -> incorrect
    + [Conferencias sobre biodiversidad.] -> correct
    + [Reuniones sobre arte y cultura local.] -> incorrect
    

=== q4
Pregunta {current_question}:<>
¿Cómo se llama el centro de entrenamiento para biomonitores?
    + [BioPark] -> correct
    + [EcoCenter] -> incorrect
    + [GreenHub] -> incorrect
    + [NatureLab] -> incorrect
    
=== q5
Pregunta {current_question}:<>
¿Qué es lo que más disfrutan los empleados de trabajar en la estación biológica?  
    +[El contacto con la naturaleza y el impacto positvo] -> correct
    +[La posibilidad de ganar premios] -> incorrect
    +[La alta remuneración] -> incorrect
    +[La competencia entre ellos] -> incorrect
    
=== q6
Pregunta {current_question}:<>
¿Qué tipo de datos recolectan actores como Alex en la estación?
    +[Datos meteorológicos] -> incorrect
    +[Datos de biodiversidad.] -> correct
    +[Datos de calidad del agua] -> incorrect
    +[Datos de plantas comestibles] -> incorrect
    