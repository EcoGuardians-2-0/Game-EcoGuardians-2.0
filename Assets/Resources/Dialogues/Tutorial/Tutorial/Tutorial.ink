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
    Hola. Perdón por no darte la bienvenida antes, mi nombre es Diego. #speaker: Diego # animation: 1
    Recuerda presionar la tecla <b><color=\#FEF445>enter</color></b> para avanzar en el dialogo.
    Utiliza las <b><color=\#FEF445>flechitas</color></b> izquierda y derecha para moverte entre las opciones.
    -> continue_dialogue
    
    

=== continue_dialogue
    ¡Muy bien! Ahora que sabes cómo interactuar con los diálogos... # animation: 5
    ¿Estás list@ para comenzar tu visita? (<color=\#FEF445A>Presiona enter</color>)
    + [No, aún no quiero empezar] ->  not_ready
    + [¡Si, quiero empezar ya!] -> start_game



=== start_game
    Perfecto. Tu primera misión sera encontrar al biomonitor de la estación.  # animation: 3
    Recuerda, puedes activar o desactivar con <color=\#FEF445>M</color> el minimapa.
    También, puedes activar o desactivar con <color=\#FEF445>TAB</color> la lista de tareas.
    Usa estas herramientas para guiarte en la estación y así puedas hablar con él.
    Tiene preparado muchas misiones para tí en este primer módulo.
    { global_tutorial_completed == false:
        ~ global_tutorial_completed = true
    }
    { already_talked == false:
        ~already_talked = true
    }
    -> additional_dialogue

=== additional_dialogue
    También te invito a tomarle fotos con la cámara a las diferentes especies de pájaros que puedes encontrar.
    Si le tomas las fotos a todas las aves, puede que las veas al final del juego... 
    Espero que disfrutes tu visita a la estación. #speaker: Diego
    -> DONE

=== not_ready
    Está bien, tómate tu tiempo. Avísame cuando quieras empezar tu vista a la estación.
    -> DONE
    
===  nodialogue
    Aún no es el momento para que hablemos... (<color=\#FEF445>Presiona enter</color>) # speaker: Diego
    -> DONE 

=== afterwards
    Te recomiendo revisar tu minimapa. El icono te ayudará a guiarte hacia la ubicación del biomonitor.  # animation: 1
    Camina hacia el icono y encontrarás al biomonitor quien te indicará tu siguiente misión en la estación.
    -> DONE

* -> END