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
    ¡Hola! Soy Ana y me encargo del Volumen de conocimiento. Aqui organizamos conferencias y talleres sobre biodiversidad y conservación. #speaker: Ana # animation: 1
    Es un espacio diseñado para que expertos y biomonitores compartan experiencias y aprendan juntos.
    ->question

=== intro2
    ¡Hola, que gusto verte de nuevo! Espero estes aprovechando al máximo tu visita. # speaker: Ana # animation: 1
    Si quieres saber más sobre algún evento, estaré encantada de ayudarte.
    ->question

=== intro3
    ¡Felicitaciones por completar el cuestionaro! Es genial ver tu interés en la estación<>
    y en el programa Awaq Bio-Tech. # speaker: Ana # animation: 1
    Si quieres saber más sobre lo que hacemos ¡Aquí estoy para resolver cualquier duda que tengas!
    ->question
    
    
=== question
    { already_talked: 
        ¿Te gustaría saber más sobre los eventos que organizamos aqu en el Volumen de Conocimiento? # animation: 5
        - else:
        Hay algo más que te gustaría saber sobre nuestro trabajo aquí en el Volumen del Conocimiento # animation: 5
    }
    + [¿Qué es el volumen de conocimiento?]
        -> q1
    + [¿Cuál es tu rol aquí?]
        -> q2
    + [No tengo más preguntas.]
        -> finish
        
=== q1
    Es una sala de reuniones y conferencias que se utiliza para <color=\#FEF445> organizar eventos de formación, capacitaciones y conferencias sobre biodiversidad y conservación</color>. # animation: 3
    +[¿Qué tipo de eventos se organizan en el Volumen de Conocimiento?]
        Organizamos <color=\#FEF445>conferencias sobre biodiversidad y talleres de formación </color>.
        Además de preparar reuniones con expertos en conservación y tecnología.
        -> question    
    +[¿Cómo contribuyen estos eventos al éxito del programa Awaq Bio-Tech?]
        Los eventos <color=\#FEF445>permiten formar los biomonitores y compartir conocimiento con expertos</color> internacionales.
        Esto ayuda a fortalecer las capacidades de nuestro equipo y mejora la implementación del programa.
        -> question
    
    
=== q2
    Mantener el equilibrio entre la conservación de la biodiversidad y el turismo sostenible ha sido un reto. # animation: 3
    +[¿Qué desafios enfrentas al organizar eventos aquí?]
        El mayor desafío es asegurarnos que la logística funcione sin problemas.
        En especial, cuando tenemos eventos con muchos asistentes o expertos de distintas áreas.
        -> question    
    +[¿Qué es lo que más disfrutas de tu trabajo?]
        Me apasiona el trato al cliente y la organización de eventos.
        Me encanta ver cómo las personas aprenden y se inspiran durante las conferencias.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_3 == true:
      ~global_mision_completada="quest_3.2"
    }
    ¡Fue un placer hablar contigo! No dudes en volver si tienes más preguntas.
    ¡Nos vemos pronto!
    -> DONE

* -> END
    



