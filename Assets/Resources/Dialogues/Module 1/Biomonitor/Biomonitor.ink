INCLUDE ../../Globals/globals.ink
INCLUDE Questionnaire1.ink
INCLUDE ../../Random/RandomBioMonitor.ink

VAR already_talked = false
VAR failed_test = 0
~global_cuestionario_1 = true

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
    ¡Hola! Soy Carlos, veo que estas visitando nuestra estación  por primera vez y noot que te gustaría conocerla. #speaker: Carlos # animation: 1
    Primero que todo, nos encontramos en Aguadas, Caldas. En la Estación Biológica del Norte de Caldas o por sus siglas, EBNC.
    Vamos a hacer esto. Te ayudaré a explorarla, mientras que cumplas determinadas tareas y retos que te colocaré para que sea más divertido.
    Tengo una idea: ¿por qué no hablas con mis compañeros en sus oficinas aquí abajo? Cuando regreses, te preguntaré sobre lo que hacen en la estación.
    // Assigning missions to character
    ~ global_misiones_1 = true
    // Marking character as already talked
    ~ already_talked = true
    ->DONE

=== second_time
   -> ChooseRandomDialogueBio1


=== suerte
    Dirigite al siguiente módulo, allí encontrás a más de mis compañeros.
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
        ~global_pass_1 = true
        ~global_mision_completada = "questionnaire1"
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