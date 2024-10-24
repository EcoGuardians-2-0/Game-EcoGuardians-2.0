INCLUDE ../../Globals/globals.ink
INCLUDE Questionnaire3.ink
INCLUDE ../../Random/RandomBioMonitor.ink

VAR already_talked = false
VAR failed_test = 0

{ global_pass_3 == false:
    {   global_cuestionario_3:
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
    ~global_mision_completada = "quest_3.0"
    ¡Hola! Soy Luis, veo que ya has llegado al último módulo de nuestra estación. #speaker: Luis # animation: 1
    Te felicito por llegar hasta aquí. Ahora, el reto final será que realices todas las actividades del tercer módulo.
    Cuando termines todas las tareas del tercer módulo, vuelve a hablar conmigo para realizar el último cuestionario.
    Estoy seguro de que este cuestionario será el cierre perfecto para ver que tanto has conocido sobre la estación.
    No falta mencionar que fijes tu atención <color=\#FEF445>cuando el color del tono de la conversación cambia de color.</color> Será de mucha ayuda en tu <color=\#FEF445>ultimo cuestionario</color> 
    ~ already_talked = true
    ~ global_misiones_3 = true
    ->DONE

=== second_time
    ¡Hola de nuevo! #speaker: Luis
   -> ChooseRandomDialogueBio3


=== suerte
    ¡Felicitaciones por haber completado el cuestionario!. #speaker: Luis # animation: 5
    Parece que has terminado de explorar toda la estación y de conocer todo lo que Awaq ofrece tanto a sus visitantes como a sus colaboradores.
    Solo me resta preguntarte lo siguiente...
    * -> final_question
    
=== final_question
<color=\#d90429>¿Estás listo para terminar tu experiencia en la estación biológica?</color>
    +[Si, siento que he recorrido bastante de la estación]
        Vale, me ha sido un placer conocerte durante tu visita por la <color=\#70e000>estación biológica de Aguadas Caldas</color>.
        Esperamos que regreses pronto. !Estamos con los brazos abiertos!
        -> finish_game
    +[No, aún siento que me queda algo por explorar.]
        Okay, no te preocupes, puedes seguir conversando con nuestro personal o jugar de nuestros minijuegos.
        ¡Sientete como en casa!
        -> DONE
        
=== finish_game
~ global_finished_game = true
-> DONE

=== cuestionario ==
    Hola ¿Cómo te ha ido explorando esta parte de la estación? # speaker: Luis # animation: 1
    {   failed_test > 0:
            Veo que ya habías intentado el cuestionario antes.<>
            No te preocupes, ¡Se que esta vez lo pasaras!
            ¿Te gustaría intentarlo de nuevo o prefieres esperar un poco más?
                +[Claro]
                    ¡Perfecto! Comencemos con el cuestionario. Estoy seguro de que lo harás mucho mejor esta vez.
                    ->start->evaluacion
                +[En un rato lo haré gracias]
                    Está bien, no hay prisa. Cuando te sientas listo,estaré aquí para el cuestionario. 
                    ¡Solo házmelo saber!
                    -> DONE
        -else:
            ...
            ¡Genial, ya hablaste con mis compañeros! 
            Como te habia comentado, prepare un cuestionario para ver que tanto has conocido de la estación.
            ¿Te sientes preparado para hacerlo ahora?
                +[Claro]
                    ¡Excelente decisión! Estoy seguro de que lo harás muy bien.
                    ->start->evaluacion
                +[En un rato lo haré gracias]
                    Está bien, no hay problema. Si prefieres pensarlo un poco más, aquí estaré cuando estés listo para intentarlo.
                    -> DONE
    }
    
== evaluacion
    {score>= passing_score:
        ~global_mision_completada = "quest_3.0"
        {score == total_questions:
            ¡Increíble! Has respondido todas las preguntas correctamente y pasaste el cuestionario del tercer módulo.
        -else:
            Has pasado el cuestionario del tercer modulo. ¡Felicitaciones!
        }
        Eres extraordinari@, <color=\#FEF445> has completado todos los retos y actividades que hemos preparado </color>.
        Si deseas, puedes continuar explorando otros rincones de la estación que te faltaron visitar.
        Pero si sientes que has terminado, <color=\#d90429>vuelve hablar conmigo para salir de la estación biológica y terminar el juego</color>.
        ~global_pass_3 = true

     -else:
        ~failed_test++
        Cuando te sientas preparado nuevamente puedes volver <> 
        a hacer el cuestionario para continuar.
    }
    ~score = 0
    *->DONE
    

* -> END