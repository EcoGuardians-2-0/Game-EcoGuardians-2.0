INCLUDE ../../Globals/globals.ink

VAR already_talked = false

{ already_talked:
        -> intro2
    - else:
        -> intro
}

=== intro
    ¡Hola! Soy Laura, la administradora de esta estación. <> Me encargo de que todo funcione sin problemas. #speaker: Lala #animation: 1
    -> question

=== intro2
    ¡Hola de nuevo! ¿Cómo la estás pasando? #speaker: Lala # animation: 1
    -> question
    
=== question
    ¿Hay algo en lo que te pueda ayudarte? #animation: 5
    + [¿Qué implica ser la administradora de la estación?]
        -> q1
    + [¿Cuál ha sido el mayor desafio en tu trabajo aquí?]
        -> q2
    + [Estoy bien, gracias]
        -> finish
        
=== q1
    Como administradora, me encargo de la logística, la asignación de recursos, y la coordinación del personal. #animation: 3
    Básicamente, me asegura que todo este en su lugar y todos tengan lo que necesitan.
    ->question
    
=== q2
    El mayor desafío ha sido lidiar con las emergencias imprevistas como cuando se interrumpieron las comunicaciones con el exterior. #animation: 3
    ->question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_2"
    }
    ¡No dudes en venir a verme <>
    si tienes más preguntas o necesitas algo!
    -> DONE

* -> END
    



