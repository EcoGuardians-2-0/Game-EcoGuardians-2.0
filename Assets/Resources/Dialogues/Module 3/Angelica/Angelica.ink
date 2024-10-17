INCLUDE ../../Globals/globals.ink

VAR already_talked = false

// Speaker's name at the start of the conversation
// Check if player's has already talked with Jenny before
{ already_talked:
        {  global_pass_3:
                -> intro3
            - else:
                -> intro2
        }
    - else:
    -> intro
} 

=== intro
    ¡Hola! Soy Angélica, la directora de la estación biológica. #speaker: Angélica # animation: 1
    Llevo tres años trabajando aquí y mi principal misión es asegurar que la estación mantenga sus funciones de conservación.
    ->question

=== intro2
    ¡Hey! ¡Mira quién ha vuelto! Me alegra que regreses. # speaker: Angélica # animation: 1
    Estoy aquí para resolver cualquier inquietud que tengas sobre la base de operaciones y mis funciones en el trabajo.
    ->question

=== intro3
    ¡Hola, qué alegría verte de nuevo y con tan buen resultado en el cuestionario! #speaker: Angélica # animation: 1
    Si quieres saber más sobre el trabajo que hacemos o cómo hemos transformado la estación, estoy aquí para responder tus preguntas.
    ->question
    
    
=== question
    { already_talked:
        ¿Te gustaría saber más sobre lo que hacemos en la estación? # animation:5
        - else:
        ¿Hay algo en especifico de lo que te gustaría hablar? # animation: 5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Qué desafíos has enfrentado en la dirección de la estación?]
        -> q2
    + [No tengo más preguntas.]
        -> finish
        
=== q1
    Mi principal misión es asegurar que la estación mantenga sus funciones de conservación y que siga siendo un centro cultural y de biodiversidad. # animation: 3
    +[¿Cuál es el rol del BioPark dentro del programa Awaq Bio-Tech?]
        Aquí entrenamos a los biomonitores en técnicas de monitoreo de biodiversidad, y recolectamos datos valiosos para el programa Awaq Bio-Tech.
        -> question    
    +[¿Cómo has logrado transformar la estación en estos tres años?]
        He trabajado para que la estación mantenga su estructura original, tanto cultural como ambiental.
        También hemos creado el <color=\#FEF445>BioPark</color>, que es nuestro <color=\#FEF445>centro de entrenamiento para biomonitores</color>.
        -> question
    
    
=== q2
    Mantener el equilibrio entre la conservación de la biodiversidad y el turismo sostenible ha sido un reto. # animation: 3
    +[¿Qué impacto tiene la estación en la comunidad?]
        La estación es un motor de desarrollo para la comunidad.
        Ofrecemos <color=\#FEF445>empleo, capacitación, y fomentamos el turismo responsable</color>, lo que beneficia a todos los que viven en la región.
        -> question    
    +[¿Cómo impacta tu trabajo en la conversación de la biodiversidad?]
        Creo que seguiremos creciendo como un referente en la conservación y en el turismo sostenible.
        Mi meta es que la estación sea reconocida a nivel internacional por su impacto positivo.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_3 == true:
      ~global_mision_completada="quest_3.1"
    }
    Vale, estaré aqui por si me necesitas o tienes más preguntas.
    -> DONE

* -> END
    



