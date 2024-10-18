INCLUDE ../../Globals/globals.ink

VAR already_talked = false

{ global_tutorial:
    {   already_talked:
            -> afterwards
        -else:
            -> tutorial
    }
    - else:
        -> nodialogue
}

=== tutorial 
    Hola. Perdón no saludarte antes, mi nombre es Diego. #speaker: Diego # animation: 1
    Recuerda presionar la tecla <b><color=\#FEF445>enter</color></b> para avanzar en el dialogo.
    Utiliza las <b><color=\#FEF445>flechitas</color></b> izquierda y derecha para moverte entre las opciones.
    -> continue_dialogue
    
    

=== continue_dialogue
    ¡Muy bien! Ahora que sabes como interactuar con los diálogos... # animation: 5
    ¿Estás list@ para comenzar tu visita?
    + [No, aún no quiero empezar] ->  not_ready
    + [¡Si, quiero empezar ya!] -> start_game



=== start_game
    Perfecto. Tu primera misión sera encontrar al biomonitor de la estación.  # animation: 3
    Recuerda revisar tu minimapa para poder ubicarlo y hablar con el.
    Tiene preparado muchas misiones para tí en este primer módulo.
    { global_tutorial_completed == false:
        ~ global_tutorial_completed = true
    }
    { already_talked == false:
        ~already_talked = true
    }
    -> additional_dialogue

=== additional_dialogue
    Tambien te invito a tomarle fotos con la camara a las diferentes especies de pajaros que puedes encontrar.
    Si le tomas las fotos a todas las aves, puede que las veas al final del juego... 
    Espero que disfrutes tu visita a la estación. #speaker: Diego
    -> DONE

=== not_ready
    Esta bien tomate tu tiempo. Avisamo cuando quieras empezar tu vista a la estación.
    -> DONE
    
===  nodialogue
    ... # speaker: Diego
    -> DONE 

=== afterwards
    Te recomiendo revisar tu minimapa. El icono te ayudará a guiarte hacia la ubicación del biomonitor.  # animation: 1
    Camina hacia el icono y encontraras al biomonitor quien te indicara tu siguiente misión en la estación.
    -> DONE

* -> END