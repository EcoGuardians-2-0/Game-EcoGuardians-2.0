INCLUDE ../../Globals/globals.ink

VAR already_talked = false

// Speaker's name at the start of the conversation
// Check if player's has already talked with Jenny before
{ already_talked:
        {  global_pass_2:
                -> intro3
            - else:
                -> intro2
        }
    - else:
    -> intro
} 

=== intro
    ¡Hola, soy Luisa!Encantada de conocerte. Soy la chef del restaurante de la estación. #speaker: Luisa # animation: 1
    Estaré aqui para cualquier cosa que necesites sobre la comida o el restaurante.
    ->question

=== intro2
    ¡Hola otra vez! Que bueno verte de nuevo por la cocina.  #speaker: Luisa # animation: 1
    Siempre hay nuevos sabores que descubrir al explorar la estación.
    ->question

=== intro3
    ¡Hola otra vez, felicitaciones por el cuestionario, lo sabias todo! #speaker: Luisa # animation: 1
    Si quieres saber más sobre lo que hacemos o simplemente disfrutar de un buen plato, ¡Aquí estoy!
    ->question
    
    
=== question
    { already_talked:
        ¿Te gustaría saber sobre mi trabajo o el restaurante? # animation:5
        - else:
        ¿Te gustaría seguir charlando sobre mi trabajo en la cocina o algun detalle del restaurante? # animation:5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Qué impacto tiene el restaurante en la comunidad?]
        -> q2
    + [No estoy bien, voy a seguir explorando]
        -> finish
        
=== q1
    Mi misión es <color=\#FEF445>promover la cocina local de las arrierías</color> y ofrecer una experiencia gastronómica única a los visitantes. # animation: 3
    +[¿Cómo contribuye el restaurante al éxito del BioPark y del programa Awaq Bio-Tech?]
        Ofrecemos una experiencia gastronómica que complementa las actividades del BioPark.
        También a disfrutar de la <color=\#FEF445>comida local, lo que ayuda a generar más ingresos y fortalecer el turismo sostenible</color>
        -> question    
    +[¿Qué es lo que más te gusta de trabajar en la estación biológica?]
        Me encanta la diversidad de personas y culturas que pasan por el restaurante.
        También disfruto saber que estoy contribuyendo al desarrollo local a través de la cocina.
        -> question
    
    
=== q2
    <color=\#FEF445>Capacitamos a mujeres locales</color>, dándoles la oportunidad de aprender nuevas habilidades culinarias y empoderarlas para mejorar su situación económica. #animation: 3
    +[¿Cómo manejas la sostenibilidad en la cocina?]
        Utilizamos <color=\#FEF445>ingredientes frescos y locales</color>, muchos de ellos obtenidos de huertos orgánicos cercanos.
        También minimizamos los residuos y promovemos prácticas de cocina sostenible.
        -> question    
    +[¿Cómo te coordinas con el resto del equipo de la estación?]
        Trabajo de cerca con el equipo administrativo y los guías turísticos para asegurarme de que los visitantes tengan una experiencia completa
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_2 == true:
      ~global_mision_completada="quest_2.1"
    }
    Si necesitas ayuda o tienes alguna pregunta en el futuro <br> 
    ¡No dudes en acercarte!
    -> DONE

* -> END
    



