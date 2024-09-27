INCLUDE ../../Globals/globals.ink
INCLUDE Questionnaire1.ink

VAR already_talked = false
VAR failed_test = 0

{ global_pass_1 == false:
    {   global_cuestionario_1:
        -> cuestionario
        -else:
        {   already_talked: 
             -> second_time
            -else:
             -> intro
        }
    }
    -else:
        ->suerte
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
   ¡Hola de nuevo! ¿Cómo te fue con mis compañeros? #speaker: Carlos #animation:1
   -> question

=== question
    Dime, ¿Tienes alguna pregunta? # animation: 3
   + [Todo bien, ¿qué haces aquí?]
        -> work_question
   + [Nada más, adiós]
        -> end_dialogue    
   
=== work_question
    Soy responsable de monitorear la salud de todos los seres vivos en esta estación. # animation: 5
    -> question

=== end_dialogue
    ¡Hasta luego! <>
    Recuerda hablar con mis otros compañeros para cumplir el reto.
    * -> DONE

=== suerte
    Dirigite al siguiente módulo, allí encontrás a más de mis compañeros.
    * -> DONE
    
=== cuestionario ==
    {   failed_test > 0:
            Has vuelto a intentar el cuestionario, te deseo mucha suerte.#speaker: Carlos # animation: 1
        -else:
            Veo que estás preparado para realizar el cuestionario #speaker: Carlos # animation: 1
    }
    ->start->evaluacion

== evaluacion
    {score>= passing_score:
        ~ global_pass_1 = true
        !Felicidades has pasado el primer cuestionario!
        Sigue las escalera y entraras al restaurante, alli podras conocer al chef.
     -else:
        ~failed_test++
        Lo siento pero no has alcanzado el puntaje suficiente.
        Puedes volver a intentarlo de nuevo.
    }
    ~score = 0
    *->DONE
    

* -> END