VAR dialogueNumber = 0

=== ChooseRandomDialogueBio1
~ dialogueNumber = RANDOM(0,3)
{ dialogueNumber:
    -1: -> RANDOM1
    -2: -> RANDOM2
    -3: -> RANDOM3
    -else: ->DONE
}


=== RANDOM1
¡Hola de nuevo! Te sugiero que hables primero con mis compañeros. # animation:1
Ellos pueden darte más detalles. Después, estaré encantado de continuar nuestra charla.
-> DONE

=== RANDOM2
¡Qué bueno verte de nuevo! ¿Podrías hablar primero con mis compañeros? # animation:1
Ellos te explicarán mejor lo que hacemos aquí. Luego seguimos conversando. 
-> DONE

=== RANDOM3
¡Qué bueno que volviste! Pero antes de seguir... # animation:1
¿Por qué no hablas con mis compañeros? Después podemos conversar con más calma.
-> DONE

->END