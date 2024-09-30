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
    ¡Hola! Bienvenido al Café Mirador. Mi nombre es Renato y soy el manager de este lugar. # speaker:Renato # animation: 1
    Este es el sitio perfecto para relajarte después de explorar la estación.
    ->question

=== intro2
    Me alegra verte de nuevo por aquí!<>
    Espero que estés disfrutando de tu visita # speaker:Renato # animation: 1
    Recuerda que siempre es un buen momento para una taza de café y un rato de observación de aves.
    ->question

=== intro3
    !Felicitaciones por completar el cuestionario!<>
    Eso muestra tu interés y dedicación # speaker:Renato #animation:1
    Si alguna vez necesitas un descanso estaré aquí con una taza de cafe recién hecho.
    ->question
    
    
=== question
    { already_talked:
        ¿Tienes alguna pregunta sobre el Café Mirador?<>
        Me encantaría contarte sobre las aves que puedes ver aquí y las delicias que servimos. # animation:5
        - else:
        ¿Te gustaría saber más sobre el café mirador o alguna de las especies que puedes avistar?# animation:5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Qué tipo de bebidas y alimentos se sirven en el café?]
        -> q2
    + [No tengo más preguntas.]
        -> finish
        
=== q1
    Soy el manager del café mirador, un lugar priviliegado dentro de la estación que <>
    ofrece vistas espectaculares y un espacio para el avistamiento de aves. # animation: 3
    +[¿Qué tipos de aves se pueden ver desde el café?]
        Las más comunes que se pueden son <color=\#FEF445>los tucanes, colibríes y águilas </color>.
        Cada mañana es una oportunidad para descubrir nuevas especies.
        -> question    
    +[¿Qué tipo de experiencias ofrecen a los visitantes que se interesan por el avistamiento?]
        Ofrecemos sesiones guiadas de avistamiento al amanecer.
        Explicamos las mejores técnicas para observar aves sin perturbar su habitat.
        -> question
    
    
=== q2
    Ofrecemos café local de alta calidad, cultivado en la región, junto con repostería casera y platos ligeros. # animation: 3
    +[¿Cómo combinas tu pasión por el avistamiento de aves con tu trabajo?]
        Suelo compartir mi conocimiento sobre las aves con los visitantes.
        Es un gran placer hablarales sobre las especies que pueden observar mientras disfrutan su café.
        -> question    
    +[¿Cómo impacta tu trabajo en la conversación de la biodiversidad?]
        Al educar a los visitantes sobre las aves locales y ayudamos a generar conciencia sobre la importancia de proteger su habitat
        Además, los ingresos del café contribuyen a los proyectos de conservación de la estación.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_1.5"
    }
    Fue un placer charlar contigo. ¡Espero que disfrutes de tu visita a la estación!<br>
    Recuerda que siempre hay una taza de café y un paisaje espectacular.
    -> DONE

* -> END
    



