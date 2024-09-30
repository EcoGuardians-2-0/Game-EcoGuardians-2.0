INCLUDE ../../Globals/globals.ink

VAR already_talked = false

// Speaker's name at the start of the conversation
// Check if player's has already talked with Jenny before
{ already_talked:
        {  global_pass_1:
                -> intro3
            - else:
                -> intro2
        }
    - else:
    -> intro
} 

=== intro
    ¡Hola! Soy Angie, la encargada de la información turística en la estación # speaker: Angie # animation: 1
    Estoy aqui para ayudarte a descubrir todo lo que esta hermosa región tiene para ofrecer.
    ->question

=== intro2
    ¡Qué gusto verte de nuevo! Espero que estés disfrutando el tiempo aquí. # speaker: Angie # animation: 1
    Recuerda que ofrecemos información sobre las actividades turísticas que puedes disfrutar.
    ->question

=== intro3
    ¡Hola de nuevo! Quiero felicitarte por completar el cuestionario # speaker: Angie # animation: 1
    Es genial ver a visitantes tan comprometidos en explorar. <>
    Espero que hayas encontrado la información útil.
    ->question
    
    
=== question
    { already_talked:
            ¿Sobre que tema te gustaría hablar de información turística? # animation:5
        - else:
            ¿Hay más preguntas que tengas sobre información turística? # animation:5
    }
    + [¿Cuál es tu función principal en la estación biológica?]
        -> q1
    + [¿Cómo trabajas con el resto el equipo de la estación?]
        -> q2
    + [No estoy bien, voy a seguir explorando]
        -> finish
        
=== q1
    Vender las entradas para el BioPark e información sobre actividades turísticas de la región. # animation: 3
    +[¿Qué tipo de actividades se pueden hacer?]
        Los visitantes pueden disfrutar de una variedad de actividades.
        Puedes hacer <color=\#FEF445>senderismo, caminatas a caballo y recorrido por los paisajes naturales </color>.
        -> question    
    +[¿Cómo guias a los turistas para aprovechar al máximo su visita?]
        Les doy recomendaciones sobre las mejoras rutas y actividades según sus intereses.
        Me aseguro de que tengan toda la información para disfrutar de su visita de forma segura.
        -> question
    
    
=== q2
    Nos coordinamos con todas las áreas para asegurar que los turistas tengan una experiencia fluida. # animation: 3
    +[Y...¿Cómo promueves el turismo responsable?]
        Siempre recordamos a los visitantes que respeten el entorno natural y sigan las rutas establecidas.
        Así evitamos que se interfiera a la fauna local ya que la conservación es nuestra prioridad.
        -> question    
    +[¿Qué es lo que más te gusta de la estación?]
        Me encanta poder enseñar a la gente lo hermoso del municipio.
        Así como contribuir en su desarrollo y conversación a través de mi trabajo.
        -> question
    
=== finish
    { already_talked == false :
        ~already_talked = true
    }
    
    { global_misiones_1 == true:
      ~global_mision_completada="quest_1.4"
    }
    Bueno, fue un placer hablar contigo. <> 
    No dudes en regresar si necesitas más información o recomendaciones.
    ¡Cuidate y hasta pronto!
    -> DONE

* -> END
    



