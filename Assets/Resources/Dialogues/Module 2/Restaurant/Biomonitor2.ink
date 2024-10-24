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
    ¡Hola! Soy Sara, me alegra ver que has llegado al segundo módulo, ¡Espero estés disfrutando tu experiencia! # speaker: Sara # animation: 1
    Vamos a hacer esto. Seguiremos explorando, y mientras, tú seguirás cumpliendo determinadas tareas y retos para que sea más divertido.
    Tengo una idea, ¿por qué no vas a la cocina y hablas con Luisa? Cuando regreses, te preguntaré sobre lo que ella hace en la estación.
    Recuerda, poner atención al <color=\#FEF445>color de ciertas palabras cuando hables con mis compañeros</color>, ya que serán claves durante mi <color=\#FEF445>actividad del cuestionario</color>.
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
    Parece que has completado todas las actividades de este módulo.
    Ahora, ve hacia el auditorio. Allí se encuentra <color=\#FEF445>la base de operaciones (BOP)</color> y <color=\#FEF445>el volumen de conocimiento</color>.
    Ahí encontrarás mis otros compañeros quienes tienen más actividades preparadas para ti.
    * -> DONE
    
=== cuestionario ==
    Hola. ¿Cómo te ha ido visitando el modulo? # speaker: Sara # animation: 1
    {   failed_test > 0:
            Veo que ya habías intentado el cuestionario antes.<>
            No te preocupes, ¡sé que esta vez lo pasarás!
            ¿Te gustaría intentarlo de nuevo o prefieres esperar un poco más?
                +[Claro]
                    ¡Perfecto! Comencemos con el cuestionario. Estoy seguro de que lo harás mucho mejor esta vez.
                    ->start->evaluacion
                +[En un rato lo haré gracias]
                    Está bien, no hay prisa. Cuando te sientas listo, estaré aquí para el cuestionario. 
                    ¡Solo házmelo saber!
                    -> DONE
        -else:
            ...
            ¡Genial, ya hablaste con mis compañeros! 
            Como te habia comentado, prepare un cuestionario para ver que tanto has conocido de la <color=\#70e000>estación</color>.
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
        ~global_mision_completada = "quest_2.0"
        {score == total_questions:
            ¡Increíble! Has respondido todas las preguntas correctamente y pasaste el cuestionario del segundo módulo.
        -else:
            Has pasado el cuestionario del segundo modulo. ¡Felicitaciones!
        }
        Saliendo del restaurante y caminando recto, podrás encontrar el tercer módulo de la estación.
        Este módulo contiene <color=\#FEF445>la base de operaciones y nuestro volumen de conocimiento</color> como centro de <color=\#FEF445>conferencias científicas y entrenamiento</color> para nosotros los biomonitores.
        ~global_pass_2 = true
     -else:
        ~failed_test++
        Cuando te sientas preparado nuevamente puedes volver <> 
        a hacer el cuestionario para continuar.
    }
    ~score = 0
    *->DONE
    

* -> END