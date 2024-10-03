INCLUDE ../../Globals/globals.ink
INCLUDE Questionnaire2.ink
INCLUDE ../../Random/RandomBioMonitor.ink

VAR already_talked = false
VAR failed_test = 0

{ global_pass_2 == false:
    {   global_cuestionario_2:
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

// Sara's introduction
=== intro
    ~global_mision_completada = "quest_2.0"
    ¡Hola! Soy Sara, me alegra ver que haz llegado al segundo modulo, ¡Espero estes disfrutando tu experiencia! # speaker: Sara # animation: 1
    Vamos a hacer esto. Seguiremos explorando, mientras seguirás cumpliendo determinadas tareas y retos para que sea más divertido.
    Tengo una idea: ¿por qué no vas a la cocina y hablas con Luisa? Cuando regreses, te preguntaré sobre lo que hace en la estación.
    // Assigning missions to character
    ~ global_misiones_2 = true
    // Marking character as already talked
    ~ already_talked = true
    ->DONE

=== second_time
    ¡Hola de nuevo! #speaker: Sara
   -> ChooseRandomDialogueBio2


=== suerte
    ¡Felicitaciones por haber completado el cuestionario!. #speaker: Sara # animation: 5
    Parace que has completado todas las actividades de este modulo.
    Ahora, ve hacia al auditorio. Ahí encontraras mis otros compañeros quienes tienen más actividades preparadas para ti.
    * -> DONE
    
=== cuestionario ==
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
        ~global_pass_2 = true
        ~global_mision_completada = "quest_2.0"
        ¡Increíble! Has respondido todas las preguntas correctamente.<>
        Has pasado el cuestionario del primer modulo.
        Subiendo las escaleras te encontrás el segundo modulo.<>
        El restaurante y baño público hacen parte del segundo modulo.
     -else:
        ~failed_test++
        Cuando te sientas preparado nuevamente puedes volver <> 
        a hacer el cuestionario para continuar.
    }
    ~score = 0
    *->DONE
    

* -> END