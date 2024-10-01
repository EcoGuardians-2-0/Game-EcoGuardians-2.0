VAR dialogueNumber = 0

=== ChooseRandomDialogueBio1
~ dialogueNumber = RANDOM(1,3)
{ dialogueNumber:
    -1: -> RANDOM1
    -2: -> RANDOM2
    -3: -> RANDOM3
    -else: ->DONE
}


=== RANDOM1
Te sugiero que hables primero con mis compañeros. # animation:1
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

=== ChooseRandomDialogueBio2
~ dialogueNumber = RANDOM(1,3)
{ dialogueNumber:
    -1: -> RANDOM1BIO2
    -2: -> RANDOM2BIO2
    -3: -> RANDOM3BIO2
    -else: ->DONE
}

=== RANDOM1BIO2
Te sugiero que hables primero con mi compañera. # animation: 1
Ella puede darte más detalles. Después, estaré encantado de continuar nuestra charla.
-> DONE

=== RANDOM2BIO2
¡Qué bueno verte de nuevo! ¿Podrías hablar primero con mi compañera Luisa? # animation: 1
Ella te explicara mejor lo que hacemos aquí en la cocina. Luego seguimos conversando.
-> DONE

=== RANDOM3BIO2 
¡Qué bueno que volviste! Pero antes de seguir... # animation: 1
¿Por qué no hablas con mi compañera Luisa en la cocina? Después podemos conversar con más calma.
-> DONE


->END