# Second_Project_Dominoes
![](Screenshots/Front.png)

***Proyecto de Programación II. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2021.***

Dominoes es una aplicación que permite jugar Dominoes contra una serie de "inteligencias" *lo original* de esta aplicacion es que hemos tratado de crear codigo de manera que implemetar una nueva variante de domino o una nueva inteligencia requiera de la menor cantidad de cambios posibles.

Es una aplicación de Consola, desarrollada con tecnología .NET Core 6.0 y en el lenguaje C#.

La aplicación está dividida en dos componentes fundamentales:

- `VisualDominoes` es un aplicacion de consola que renderiza la interfaz gráfica y sirve los resultados.
- `DominoEngine` es una biblioteca de clases donde estácimplementada lo neceario para la lógica del del juego.

# Sobre el Juego
**Manual de Usuario**

- El Juego comienza con una presentacion luego mostrara una serie de opciones donde usted modelara el juego.

- Primera opcion es la version de domino que desea jugar lo que es lo mismo que la cantidad de fichas con las que desea jugar debera elegir uno de los numeros que indican al lado de la version.

  - 0 Doble 7 
  - 1 Doble 8
  - 2 Doble 9
  - 3 Doble 10

- Segunda opcion debe elegir un numero que representa  la cantidad de jugadores

- Tercera opcion la cantidad de fichas por jugador

- Curata opcion tipo de juego(Constamos con tres variaciones de domino)

  - 0 Classic Dominoes (Domino Clasico)
  - 1 Pretty Boy (Domino de figuritas)
  - 2 Sloten (Domino Robaito)

- La Quinta opcion permite seleccionar el tipo de player que jugara; cada vez que seleccione un jugador este le pedira un identificador o nombre

  - 0 Human Player (Para que el usuario pueda jugar)
  - 1 Ramdom Player (Jugador Ramdom)
  - 2 Drop Fat Player (Botagordas)
  - 3 Almost Clever Player (Casi Intelijente :))

**Luego que halla seleccionado conformado el juego comenzara el el juego**
 
>La parte visual se compone de dos partes una que muestra la meas y la mano del jugador actual y otra que muestara la mano de todos los jugadores

![](Screenshots/PrintGame.png)

![](Screenshots/Hands.png)


# Sobre la ingeniería de software

**Para implementar la lógica del los juegos hemos implementado varias varias interfaces fundamentales:**

- `IEndConditions` contiene un metodo que basicamente dice si se cumplen las condiciones para que halla  final del juego.
- `IWinConditions` tiene un metodo que basicamente dice si un jugador cumple las condiciones de gandor.
- `IValue`  esta interface tiene un parametreo generico *value* que basicamete es el valor de una de las caras de las fichas estas interface implementa `IEquatable<IValue<T>>` que me permite determinar si dos `IValue` son iguales.
- `IStrategy` esta interface tiene un metodo que dice si puede jugar o no y asigna la ficha y la cara  por donde desea jugar la ficha [`out (Chip<TValue, T>,TValue)`]
- `IRankeble` nos permite implementar una valor de cara (IValue) que pueda asignar un metodo racking de que tan grande es ese valor la utilidad de esto es que permitira hacer que un jugador botgorda pueda jugar con tipos de IValues no numericos

- `IGameLogic` permite implementar logicas de juegos muy distintas una de la otra esta consta de:

```cs
  public interface IGameLogic<TValue,T> where TValue:IValue<T>
    {
        int Turn { get; } // Turno Actual del juego
        Board<TValue, T> board { get; } //Una Mesa
        List<Player<TValue, T>> Players { get; }//Lista de Jugadores
        Rules<TValue, T> Rules { get; }//Las reglas del juego
        List<Chip<TValue, T>> Chips { get; }//Las fichas
        Player<TValue, T>? CurrentPlayer { get; }//Un Jugador Actual;
        
        //Un metodo que busca en la lista un jugador que pueda jugar y hace el proceso de pasar el turno y declarar al jugador como pasdo si a este le toca y no puede jugar
        void ChangeValidCurrentPlayer();
       
        //Ejecuta las cosas que debe parar en el turno del jugador actual
        void CurrentTurn();
        //Dice si el Juego se termino
        bool EndGame();
        //Reparte de forma un numero de fichas
        void HandOutChips(int CountChip);
    }
```
**El resto de las clases implementan estas interface para crear distintos criterios para la conformacion de un juego**

**Tenemos clases que no implementan de  ninguna interface puesto que no cosideramos necesario la variacion de ellas de forma absoluta**

Encontramos la clase player que solo varia algunas de sus partes como lo es la estrategia que es pasada a este y encapsulada con un metodo nextPlay

Encotramos Rules que los apectos a variar de ella son las condiciones de final empate y ganador y esto son determinados y pasados a esta como condiciones que permiten identificar esos estados

# Posibles mejoras a deficiencias 
- Como posibles mejoras esta la optimización e implementacion de mas variantes de juegos.

- Reajustar el funcionamiento y aumentar la abtracion par implemetar juegos mas potentes.

- Implementar un jugador de domino mas intelligente que los acuales.

- Implementar una interfaz gráfica. 


