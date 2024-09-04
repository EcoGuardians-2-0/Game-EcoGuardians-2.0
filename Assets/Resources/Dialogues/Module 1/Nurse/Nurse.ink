INCLUDE ../../Globals/globals.ink

VAR already_talked = false

{ already_talked:
    -> intro2
    - else:
    -> intro
}


=== intro
    Hola, Soy María, la enfermera de la estación. Estoy aquí para ayudarte con cualquier problema de salud que puedas tener. #speaker:Alexa #animation:1
    -> question

=== intro2
    ¡Hola amig@¡ ¿Cómo te ha ido?
    -> question
    

=== question

    {  already_talked:
        ¿Necesitas algo más de mi parte? #animation: 3
      - else:
        ¿De qué te gustaría hablar? #animation: 3
    }
    
    +[¿Qué es lo más inusual que has tenido que tratar?]
        -> q1
    +[¿Qué te gusta hacer cuando no estás trabajando?]
        -> q2
    +[Seguire explorando la estación]
        -> f1

=== q1
    Una vez tuve que tratar con un compañero con una rara reacción a una planta local. #animation: 5
    -> question

=== q2
    Cuando no estoy en la enfermería, me gusta explorar la fauna local.
    -> question

=== f1
    ¡Cuidate y recuerda, estoy aquí si necesitas algo! #animation: 1
    {  already_talked == false:
        ~global_mision_completada = "quest_001"
    }
    ~already_talked = true
    -> DONE

* -> END
    


