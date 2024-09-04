INCLUDE ../../Globals/globals.ink

VAR already_talked = false

{ global_cuestionario_1:
    -> cuestionario
    -  else:
    { already_talked:
        -> second_time
    - else:
        -> intro
    }
}

// Carlos' introduction
=== intro
    ¡Hola! Soy Carlos, un biomonitor de la estacion biológica. <>
    Te contare un poco sobre mí. # speaker: Carlos # animation: 1
    
    Veo que estas visitando nuestra estación por primera vez y noto <>
    que te gustaría explorar un poco.
    
    Te propongo un reto. # animation: 5
    
    ¿Por qué no hablas con mis compañeros aqui abajo en sus oficinas? Cuando <>
    regreses te preguntaré sobre lo que hacen en la estación.
    
    Si aciertas la mayoría, te dejare continuar tu recorrido.
    // Assigning missions to character
    ~ global_misiones_1 = true
    // Marking character as already talked
    ~ already_talked = true
    ->DONE

=== second_time
   ¡Hola de nuevo! ¿Cómo te fue con mis compañeros? #speaker: carlos #animation:1
   -> question

=== question
    Dime, ¿Tienes alguna pregunta? # animation: 3
    
   * [Todo bien, ¿qué haces aquí?] 
        -> work_question
   
   * [Nada más, adiós]
        -> end_dialogue    
   
=== work_question
    Soy responsable de monitorear la salud de todos los seres vivos en esta estación. <>
    Es un trabajo fascinante pero demandante. # animation: 5
    -> question

=== end_dialogue
    ¡Hasta luego! <>
    Recuerda hablar con mis otros compañeros para cumplir el reto.
    * -> DONE

    
=== cuestionario ==
    Estas listo para hacer el cuestionario.
    ->DONE
    

* -> END
    


