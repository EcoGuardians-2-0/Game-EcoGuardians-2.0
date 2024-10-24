INCLUDE ../../Globals/globals.ink

VAR already_talked = false
VAR chose_option = false

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
    ¡Hola! Soy María Isabel, encargada de los primeros auxilios en la estación biológica.# speaker:Maria Isabel # animation: 1
    Estoy aquí para asegurar que todos estén a salvo y preparados ante cualquier situación.
    ->question

=== intro2
    ¡Hola de nuevo! ¿Cómo te ha ido en la estación?. # speaker:Maria Isabel # animation: 1
    Si necesitas algún consejo sobre seguridad o primeros auxilios, estaré aquí para ayudarte.
    ->question

=== intro3
    ¡Hola otra vez! Muy buen trabajo por haber pasado el cuestionario.# speaker:Maria Isabel # animation: 1
    Estamos felices de que conozcas más sobre nuestra estación.
    ->question
    
    
=== question
    { already_talked:
        ¿En qué te gustaría que te ayudara hoy? # animation:5
        - else:
        ¿Te gustaría preguntarme algo{chose_option: más esta vez}? # animation:5
    }
    {chose_option == false:
        ~chose_option = true
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Cuál es tu responsabilidad en una emergencia?]
        -> q2
    + [No estoy bien, voy a seguir explorando]
        -> finish
        
=== q1
    Me encargo de estar disponible para cualquier emergencia médica que pueda surgir, y también de capacitar al equipo en protocolos de seguridad y atención básica. #animation: 3
    +[¿Qué tipo de emergencias manejas aquí?]
        Principalmente, lidiamos con incidentes menores como cortes, caídas o picaduras de insectos.
        Aunque estamos preparados para emergencias más graves, afortunadamente, son muy pocas.
        -> question    
    +[¿Cómo entrenas al equipo para que esten preparados ante una emergencia?]
        Realizo simulacros periodícos con todo el personal para que sepan como actuar en situaciones de emergencias.
        Les enseño desde usar un botiquín de primeros auxilios hasta técnicas básicas de reanimación.
        -> question
    
    
=== q2
    Mi principal responsabilidad es evaluar rápidamente la situación, proporcionar primeros auxilios inmediatos y coordinar con los servicios médicos locales si es necesario. #animation: 3
    +[¿Qué herramientas y recursos usas en tu trabajo diario?]
        En mi trabajo diario, utilizo una variedad de elementos del equipo de primeros auxilios.
        El botiquín de <color=\#FEF445>primeros auxilios </color>, <color=\#FEF445>las tijeras de emergencia</color>, <color=\#FEF445>los vendajes</color>, <color=\#FEF445>los guantes desechables</color> y otros dispositivos
        -> question    
    +[¿Qué es lo que más te gusta de trabajar en primeros auxilios?]
        Lo que más me gusta es saber que estoy ayudando a las personas.
        Me da tranquilidad saber que, en caso de cualquier emergencia, <>puedo marcar la diferencia y darles seguridad a los demás.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_1.1"
    }
    Si necesitas ayuda o tienes alguna pregunta sobre mis funciones...<br> 
    ¡No dudes en acercarte!
    -> DONE

* -> END
    



