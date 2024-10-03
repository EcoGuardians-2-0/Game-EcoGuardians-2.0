INCLUDE ../../Globals/globals.ink
INCLUDE Questionnaire1.ink
INCLUDE ../../Random/RandomBioMonitor.ink

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
    ~global_mision_completada = "quest_1.7"
    ¡Hola! Soy Carlos, veo que estas visitando nuestra estación  por primera vez y noto que te gustaría conocerla. #speaker: Carlos # animation: 1
    Primero que todo, nos encontramos en Aguadas, Caldas. En la Estación Biológica del Norte de Caldas o por sus siglas, EBNC.
    Vamos a hacer esto. Te ayudaré a explorarla, mientras que cumplas determinadas tareas y retos que te colocaré para que sea más divertido. # animation:3
    Tengo una idea: ¿por qué no hablas con mis compañeros en sus oficinas aquí abajo? Cuando regreses, te preguntaré sobre lo que hacen en la estación.
    Fijate en el <color=\#FEF445>color del tono en que te hablan los personajes</color>, quizas te ayudarán cuando vuelvas de realizar las actividades y resuelvas mi cuestionario. # animation:5
    // Marking character as already talked
    ~ already_talked = true
    ~ global_misiones_1 = true
    ->DONE

=== second_time
    ¡Hola de nuevo! #speaker: Carlos
   -> ChooseRandomDialogueBio1


=== suerte
    ¡Felicitaciones por haber completado el cuestionario!. #speaker: Carlos # animation: 5
    Parace que has completado todas las actividades de este modulo.
    Ahora, ve hacia el restaurante. Ahií encontraras mis otros compañeros quienes tienen más actividades preparadas para ti.
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
        ~global_mision_completada = "quest_1.7"
        ¡Increíble! Has respondido todas las preguntas correctamente.<>
        Has pasado el cuestionario del primer modulo.
        Subiendo las escaleras te encontrás el segundo modulo.<>
        El restaurante y baño público hacen parte del segundo modulo.
        ~global_pass_1 = true

     -else:
        ~failed_test++
        Cuando te sientas preparado nuevamente puedes volver <> 
        a hacer el cuestionario para continuar.
    }
    ~score = 0
    *->DONE
    

* -> END