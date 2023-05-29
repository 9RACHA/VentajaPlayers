# VentajaPlayers

P3.5 Vantaxes para os players

Modifica a práctica anterior do seguinte xeito. Os players van recibir de forma aleatoria e por tempo limitado unha vantaxe ou desvantaxe. A vantaxe é que o movmento é máis rápido e pode verse porque o player se pon de cor vermella, a desvantaxe o contrario, o player ponse vermello ou laranxa e vai máis lento. 

Por exemplo, pasados 20 seguntos o player 2 recibe unha vantaxe, ponse verde e vai máis rápido por 10 segundos. 

A idea é usar ClientRPC para informar aos clientes de se lle tocou unha vantaxe ou desvantaxe.

Entregable:

O script ou scripts relevantes
Un pequeno informe cun par de capturas que demostren que o xogo funciona e coas explicacións que consideres axeitadas
O enderezo de github que contén o proxecto

## Informe

Al ejecutar el juego desde Unity siendo el Host
![image](https://github.com/9RACHA/VentajaPlayers/assets/66274956/b686d28b-8154-43ce-8b44-468289b49f01)

Mediante la coroutina VentajaNerf() 
![image](https://github.com/9RACHA/VentajaPlayers/assets/66274956/040e5772-fde8-4447-8ce8-babdec0cf036)
Se esperaran 20s mediante -> yield return new WaitForSeconds(coroutina) que suspende la ejecución de la coroutine durante el tiempo especificado.

![image](https://github.com/9RACHA/VentajaPlayers/assets/66274956/963a7733-5b64-4573-991b-e4dd86ea46db)
Se generará un numero aleatorio entre 0 y 1



 
