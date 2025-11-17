# 🧟‍♂️ Invasão Zumbi em Narandiba

![Zumbi](Figures/0.png)

O mundo não acabou de uma vez. Ele desmoronou em silêncio — como a febre que começa com um arrepio. As cidades tombaram sem entender contra o quê lutavam. Sirenes, gritos e transmissões caóticas foram engolidas pela noite, até que restou apenas o silêncio.

Hoje, Narandiba é um labirinto de memórias mortas. Ruas vazias, carros abandonados, portas entreabertas que guardam histórias que ninguém mais contará. O vento sopra papéis, poeira e lembranças de um passado que desapareceu.

Os vivos se tornaram raros. Alguns fogem, outros se escondem. Muitos já esqueceram por que continuam. A fome, o medo e a solidão são mais fatais que os próprios mortos. A cada amanhecer, a esperança se desfaz um pouco mais.

Mas ainda existe quem resista. Quem se recuse a cair. Enquanto houver alguém caminhando pelas ruas, enquanto houver um último golpe de coragem… a história continua.

É nesse cenário que **Narandiba** se torna o palco do início — ou do fim — de tudo.

---

## 🔥 Personagem Principal

### **O Bárbaro**
- **Função:** Combatente corpo a corpo determinado a sobreviver  
- **Arma inicial:** Machado  
- **Habilidade especial:** Evoluir para o **Super Bárbaro** após derrotar vários zumbis

<p align="center">
<img src="Figures/1.png" width="300"> 
<img src="Figures/2.png" width="274">
</p>

*O Bárbaro em estado normal e transformado.*

**Objetivo:**  
Explorar as ruas destruídas de Narandiba, eliminar hordas de zumbis e resistir até o fim.

---

## 🗺️ Mapa de Narandiba

![Mapa de Narandiba](Figures/3.png)  
*Mapa TopDown utilizado na gameplay.*

---

## ⚙️ Mecânicas Principais

| **Ação**        | **Descrição**                                                    |
|-----------------|------------------------------------------------------------------|
| Movimentação    | O jogador move livremente usando **W, A, S, D**.                 |
| Ataque          | Golpes com o machado utilizando **J** ou clique do mouse.        |
| Evolução        | Após acumular kills, o personagem evolui temporariamente para o **Super Bárbaro**, com mais força e velocidade. |
| Timer           | O jogador tem **5 minutos** para sobreviver ao ataque das hordas. |

---

## 🧟 Inimigos

O jogo possui dois tipos principais de zumbis:

| **Tipo de Zumbi** | **Descrição**                                  | **Ameaça** |
|-------------------|-----------------------------------------------|------------|
| **Zumbi Lento**   | Movimentação fraca, mas aparecem em quantidade. | Média      |
| **Zumbi Rápido**  | Menos vida, porém veloz e agressivo.            | Alta       |

Os inimigos surgem de todos os lados, aproximam-se do player, param a uma distância segura e atacam com cadência constante.

---

## ⚔️ Sistema de Waves

Durante a partida:

- Hordas surgem dinamicamente ao redor do jogador  
- Cada wave aumenta:
  - Quantidade de zumbis  
  - Frequência de spawn  
  - Chance de zumbis rápidos  
- O jogador vence quando **todas as waves forem derrotadas**  
- O jogador perde se morrer ou o tempo acabar  

---

## 🎯 Final do Nível

Ao eliminar as ondas finais, o jogador avança até o ponto seguro.  
Lá, descobre que sobreviver foi apenas o primeiro passo…  
O mundo ainda não acabou — mas também não foi salvo.

---

## 🎮 Cenas do Jogo

- **MainMenuScene** — Tela inicial  
- **MainGameScene** — Gameplay em Narandiba  
- **VictoryScene** — Tela de vitória com total de kills  
- **GameOverScene** — Tela de derrota com total de kills

---

## 🧠 Game Design

**Invasão Zumbi em Narandiba** é uma aventura de ação top-down, misturando sobrevivência, combate corpo a corpo e evolução dinâmica.  
O jogador deve navegar pelas ruínas de Narandiba enfrentando hordas cada vez mais fortes, enquanto administra tempo, vida e oportunidades de evoluir.

A sensação é de tensão constante, ritmo acelerado e recompensa a cada golpe bem aplicado.

---

## 🎮 Elementos da Cena de Gameplay

- **Player:** Bárbaro com ataque corpo a corpo  
- **Evolução:** Transformação temporária para Super Bárbaro  
- **Inimigos:** Zumbis lentos e rápidos  
- **Spawner:** Waves dinâmicas aparecendo de todos os lados  
- **HUD:**  
  - Contador de kills  
  - Timer de 5 minutos  
- **Final:** Vitória ao derrotar todas as ondas, derrota ao morrer ou ao acabar o tempo  

---

