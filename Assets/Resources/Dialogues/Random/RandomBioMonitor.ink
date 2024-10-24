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
Te sugiero que hables primero con mis compañeros y realices la actividad. # animation:1
Es importante que conozcas un poco de la estación primero.
-> DONE

=== RANDOM2
¡Qué bueno verte de nuevo! ¿Ya has realizado todas las tareas de este módulo? # animation:1
Vuelve cuando hayas completado todas las tareas para que hablemos.
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
Te sugiero que hables primero con mi compañera y hagas la actividad. # animation: 1
Ella puede darte más detalles. Después, estaré encantada de continuar nuestra charla.
-> DONE

=== RANDOM2BIO2
¡Qué bueno verte de nuevo! Veo que aún no has terminado todas las actividades # animation: 1
Te recomiendo que las termines todas primero, luego seguimos conversando.
-> DONE

=== RANDOM3BIO2 
¡Qué bueno que volviste! Pero antes de seguir... # animation: 1
¿Por qué no realizas las actividaes restantes? Después podemos conversar con más calma.
-> DONE

=== ChooseRandomDialogueBio3
~ dialogueNumber = RANDOM(1,3)
{ dialogueNumber:
    -1: -> RANDOM1BIO3
    -2: -> RANDOM2BIO3
    -3: -> RANDOM3BIO3
    -else: ->DONE
}

=== RANDOM1BIO3
¡Qué alegría verte en el último módulo! # animation: 1
Antes de continuar, te sugiero que realices todas las actividades en este módulo.
-> DONE

=== RANDOM2BIO3
¡Qué bueno verte de nuevo! ¿Ya hablaste con mis compañeras? # animation: 1
No se te olvide realizar la actividad de este módulo. Es divertida.
-> DONE

=== RANDOM3BIO3
¡Me alegra que hayas vuelto! Pero antes de seguir... # animation: 1
Veo que te falta por terminar unas actividades. 
Realizaremos el cuestionario cuando hayas completado todas las actividades.
-> DONE


->END