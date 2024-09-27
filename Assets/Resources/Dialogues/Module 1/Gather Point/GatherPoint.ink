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
    ¡Hola! Bienvenidos al Punto de Encuentro. Soy Alex, guía de esta estación. #speaker: Alex # animation: 1
    Estoy aquí para ayudarte en tu visita. 
    ->question

=== intro2
    Es un placer verte de nuevo. Si hay algo más que pueda hacer por ti, no dudes en decirme. # speaker:Alex # animation: 1
    Espero que tu visita continúe siendo tan emocionante como la primera vez.
    ->question

=== intro3
    ¡Fantástico! Es un placer verte de nuevo después de un cuestionario tan bien resuelto # speaker: Alex # animation: 1
    Ya sabes que estoy aquí para ayudarte a disfrutar de tu visita al BioPark y resolver cualquier duda.
    ->question
    
    
=== question
    { already_talked:
        ¿En qué puedo ayudarte hoy? # animation:5
        - else:
        ¿Te gustaría preguntarme algo más esta vez? # animation:5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Cuál es tu responsabilidad en una emergencia?]
        -> q2
    + [No estoy bien, voy a seguir explorando]
        -> finish
        
=== q1
    Mi función es guiar a los visitantes por la estación y el BioPark, ayudándolos a ubicarse y <> 
    disfrutar de los atractivos naturales de manera segura. #animation: 3
    +[¿Cómo ayudas a los turistas a disfrutar de su visita?]
        Proporciono mapas y explico las rutas.
        Además, doy recomendaciones sobre los mejores lugares para ver animales y disfrutar el paisaje.
        -> question    
    +[¿Cómo entrenas al equipo para que esten preparados ante una emergencia?]
        El BioPark es donde realizamos nuestras actividades de biomonitoreo. Es una fuente clave de datos para Awaq Bio-Tech.
        Gracias a estos datos podemos proteger el ecosistema de manera más efectiva.
        -> question
    
    
=== q2
    En caso de un fenómeno natural, mi tarea es coordinar la evacuación y asegurar que todos estén a salvo. # animation: 3
    +[¿Qué consejo le darías a alguien que visita el BioPark por primera vez?]
        Que disfruten cada momento, pero respeten la naturaleza.
        Deben mantenerse en las rutas designadas y seguir las recomendaciones de seguridad para preservar la belleza de este lugar.
        -> question    
    +[¿Qué tipo de entrenamientos has recibido para tu trabajo?]
        He sido capacitado en monitoreo de biodiversidad, primeros auxilios, y manejo de emergencias.
        Además, continúo aprendiendo nuevas técnicas para mejorar la conservación.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_3"
    }
    Si necesitas ayuda o tienes alguna pregunta en el futuro <br> 
    ¡No dudes en acercarte!
    -> DONE

* -> END
    



