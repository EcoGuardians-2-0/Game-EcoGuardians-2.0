INCLUDE ../../Globals/globals.ink

VAR already_talked = false

// Speaker's name at the start of the conversation
// Check if player's has already talked with Jenny before
{ already_talked:
        {  global_pass_1:
                -> intro3
            - else:
                -> intro2
        }
    - else:
    -> intro
} 

=== intro
    ¡Hola! Bienvid@ a la Estación Biológica del Norte de Caldas.# speaker:Jenny # animation: 1
    Mi nombre es Jenny y soy la administradora de esta estación.<>
    Trabajo en la organización de Awaq. 
    ->question

=== intro2
    ¡Hola de nuevo! Qué alegría verte por aquí otra vez. # speaker:Jenny # animation: 1
    Siempre hay algo nuevo que descubrir al explorar la estación.
    ->question

=== intro3
    ¡Hola otra vez! Muy buen trabajo por haber pasado el cuestionario.# speaker:Jenny # animation: 1
    Estamos felices de que conozcas más sobre nuestra estación.
    ->question
    
    
=== question
    { already_talked:
        ¿Hay algo en específico de lo que te gustaría hablar? # animation:5
        - else:
        ¿En que puedo ayudarte hoy? # animation:5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Cómo influye la administración en el funcionamiento de la estación?]
        -> q2
    + [No estoy bien, voy a seguir explorando]
        -> finish
        
=== q1
    Mi función es gestionar toda la parte administrativa de la estación.#animation: 3
    +[¿Cómo te coordinas con el resto de equipo?]
        Trabajo con la directora de la estación y el equipo operativo.
        Organizamos reuniones periódicas para revisar los proyectos.
        -> question    
    +[¿Cómo apoyas a los biomonitores desde tu puesto?]
        Me encargo de que todos tengan los recursos necesarios. 
        Desde equipos y suministros hasta la logística de su desplazamiento en el biopark.
        -> question
    
    
=== q2
    La administración es clave para que todos los objetivos y recursos se alineen con el programa.#animation: 3
    +[¿Qué desafios enfrentan en la gestión de la estación]
        El mayor reto ha sido equilibrar los recursos disponibles con las necesidades del proyecto.
        Además, es importante mantener la coordinación entre áreas.
        -> question    
    +[¿Qué es lo que más te gusta de la estación?]
        Me encanta poder combinar mis habilidades de administración con mi amor a la naturaleza.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_2"
    }
    Vale, estaré aqui por si me necesitas o tienes más preguntas. <>
    ¡Disfruta tu recorrido por la estación!
    -> DONE

* -> END
    



