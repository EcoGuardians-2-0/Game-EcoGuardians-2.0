-> intro

=== intro ===
    Hola mi nombre es Monica y soy una guia en <b><color=\#00FF00>AWAQ</color></b>.<br>Estoy encantado de conocerte y espero que estes disfrutando del recorrido.#speaker:Monica #animation:1
* -> question

=== question ===
    Dime, ¿En qué te puedo ayudar? #animation:3
    + [Cuentame un chiste]
        -> JOKE
    + [¿Qué haces en el parque?]
        Ayudo a<color=\#FFFF00> resolver inquietudes</color>. 
        -> question
    + [Nada estoy bien]
        Okay estaré aquí por si me necesitas.
        -> DONE

=== JOKE ===
    -¿Qué le dijo<color=\#5D3FD3> una mora a otra </color>? #animation:5
    -¡Tu me ena-moras!
    + [Fue muy gracioso]
        -> question
    + [No me gusto el chiste]
        Okay tal vez no fue gracioso pero... #animation:3
        -> question

* -> END
    

