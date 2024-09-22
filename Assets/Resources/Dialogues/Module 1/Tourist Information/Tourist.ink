INCLUDE ../../Globals/globals.ink

VAR already_talked = false

{ already_talked:
        -> intro2
    - else:
        -> intro
}

=== intro ===
    Hola mi nombre es Sofia y soy una guia en <color=\#00FF00>AWAQ</color>. #speaker:Sofia #animation:1
* -> question

=== intro2
    ¡Hola de nuevo! ¿Vienes por mas información? #speaker: Sofia #animation:1
    -> question

=== question ===
    Dime, ¿En qué te puedo ayudar? #animation:3
    + [Cuentame un chiste]
        -> JOKE
    + [¿Qué haces en el parque?]
        Ayudo a<color=\#FFFF00> resolver inquietudes</color>.
        -> question
    + [¡Quiero participar en una actividad!]
        -> ACTIVITY
    + [Nada estoy bien]
        Okay estaré aquí por si me necesitas.
        -> DONE

=== JOKE ===
    -¿Qué le dijo<color=\#A020F0> una mora a otra </color>? #animation:5
    -¡Tu me ena-moras!
    + [Fue muy gracioso]
        -> question
    + [No me gusto el chiste]
        Okay tal vez no fue gracioso pero...
        -> question

=== ACTIVITY ===
    -¡Claro! Te tengo una actividad de resolver un rompecabezas. #animation: 3
    -¿Te gustaría participar?
    + [¡Claro!]
        -> PUZZLE
    + [Mejor no, gracias]
        No pasa nada, vuelve a decirme si quieres participar.
        -> question

=== PUZZLE ===
    - ¡Buena suerte! #animation:5
    + [Iniciar rompecabezas]
        -> CHANGE_SCENE_PUZZLE
=== CHANGE_SCENE_PUZZLE ===
    # Activity: 1
    -> DONE

=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_3"
    }
    ¡No dudes en venir a verme <>
    si tienes más preguntas o necesitas algo!
    -> DONE

* -> END
    


