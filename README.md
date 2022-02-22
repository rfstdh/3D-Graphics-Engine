# 3D Graphics Engine


## Opis | Description

:poland:

3D Graphics Engine to aplikacja konsolowa symulująca przebywanie w przestrzeni 3D poprzez zastosowanie rzutu perspektywicznego. Po uruchomieniu aplikacji pojawia się czarna scena z kamerą umieszczoną w punkcie (0,0,0). Kamerą można się ruszać w czterech kierunkach (wschód, zachód, północ, południe), obracać oraz przybliżać i oddalać. Ruch kamery ma imitować przemieszczanie się użytkownika w przestrzeni. Na scenie rozmieszczone są trójwymiarowe bryły (dla aplikacji testowej jest to sześcian, stożek oraz dwudziestościan foremny). Bryły zostały utworzone w programie Blender oraz zaimportowane jako plik .obj. Aplikacje rozpoznaje zapis w postaci pozycji krawędzi oraz wierzchołków i za pomocą metod DrawVertex oraz DrawContours rysuje bryły. Dodatkowo, bryły pokryte są algorytmem malarskim oraz zaimplementowane zostało cieniowanie płaskie z wykorzystanie pojedynczego, punktowego źródła światła.

:us:

3D Graphics Engine is a console application which goal is to simulate 3D space by using perspective projection. After launching the app, black scene and camera located at point (0,0,0) appears. Camera can be moved in all four directions (east, west, north, south), rotated and zoomed in/out. Camera movement is supposed to imitate player's movement in the space. There are different 3D objects in the scene (for testing purposes it is a cube, a cone and an icosahedron). Objects were created in Blender program and imported as .obj files. App can read this format with edges and vertexes positions and with a help of DrawVertex and DrawContours methods can draw those objects. Additionally, objects are covered with painter's algorithm and there is flat shading implementation with single, punctual light source.

## Zrzuty ekranu | Screenshots

### Pozycja startowa | Starting position
![w1](https://user-images.githubusercontent.com/43353217/155166589-e05d0b8c-5fa6-412d-9fd9-fcf62fc90b5d.PNG)

### Ruch w lewo | Left side movement
![w2](https://user-images.githubusercontent.com/43353217/155166582-035a76fb-8705-4392-816b-abce1373c7c4.PNG)

### Ruch do góry | Upside movement
![w3](https://user-images.githubusercontent.com/43353217/155166611-8e5638b9-4fb9-4b84-968e-2f5fb444224b.PNG)

### Ruch w prawo | Right side movement
![w4](https://user-images.githubusercontent.com/43353217/155166617-7f77cbe6-ea11-4520-a155-51f6ed539383.PNG)

## Sterowanie | Controls

- WSAD - ruch kamery | camera movement
- Z/X - przybliżanie/oddalanie | zooming in/out
- M - rotacja osi X | X axis rotation
- N - rotacja osi Y | Y axis rotation
- B - rotacja osi Z | Z axis rotation
