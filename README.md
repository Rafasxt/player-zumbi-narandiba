# 🧟‍♂️ Invasão Zumbi em Narandiba

![Zumbi](Figures/0.png)

O mundo não acabou de uma vez. Ele desmoronou em silêncio — como a febre que começa com um arrepio. As cidades tombaram sem entender contra o quê lutavam. Sirenes, gritos e transmissões caóticas foram engolidas pela noite, até que restou apenas o silêncio.

Hoje, Narandiba é um labirinto de memórias mortas. Ruas vazias, carros abandonados, portas entreabertas que guardam histórias que ninguém mais contará. O vento sopra papéis, poeira e lembranças de um passado que desapareceu.

Os vivos se tornaram raros. Alguns fogem, outros se escondem. Muitos já esqueceram por que continuam. A fome, o medo e a solidão são mais fatais que os próprios mortos. A cada amanhecer, a esperança se desfaz um pouco mais.

Mas ainda existe quem resista. Quem se recuse a cair. Enquanto houver alguém caminhando pelas ruas, enquanto houver um último golpe de coragem… a história continua.

É nesse cenário que **Narandiba** se torna o palco do início — ou do fim — de tudo.

---

# 🔥 Personagem Principal

## **O Bárbaro**
- **Função:** Combatente corpo a corpo focado em sobrevivência  
- **Arma:** Machado  
- **Habilidade:** Evolui temporariamente para o **Super Bárbaro** após acumular kills

<p align="center">
<img src="Figures/1.png" width="300">
<img src="Figures/2.png" width="274">
</p>

---

# 🗺️ Mapa de Narandiba

![Mapa de Narandiba](Figures/3.png)  
*Mapa TopDown utilizado na gameplay.*

---

# ⚙️ Mecânicas Principais

| **Ação**       | **Descrição**                                                            |
|----------------|--------------------------------------------------------------------------|
| Movimentação   | Player controla o Bárbaro com **W, A, S, D**.                            |
| Ataque         | Golpeia com o machado usando **J** ou clique esquerdo do mouse.          |
| Evolução       | Ao matar diversos zumbis, vira o **Super Bárbaro** por tempo limitado.   |
| Timer          | O jogador tem **5 minutos** para sobreviver às hordas.                   |

---

# 🧟 Inimigos

O jogo possui dois tipos principais de zumbis:

| **Tipo**        | **Descrição**                                 | **Ameaça** |
|-----------------|------------------------------------------------|------------|
| Zumbi Lento     | Movimentação fraca, aparecem em quantidade.    | Média      |
| Zumbi Rápido    | Ágil e agressivo, porém com menos vida.        | Alta       |

---

# ⚔️ Sistema de Waves

Durante a partida:

- Waves surgem automaticamente **ao redor do jogador**
- Cada wave aumenta:
  - Quantidade de inimigos  
  - Frequência de spawn  
  - Chance de zumbis rápidos  
- Vitória ao eliminar todas as ondas  
- Derrota ao morrer ou deixar o tempo acabar  

---

# 🎯 Final do Nível

Ao sobreviver a todas as waves, o jogador alcança o ponto seguro e garante a vitória.  
Mas a história… apenas começou.

---

# 🎮 Cenas do Jogo

- **MainMenuScene** — Menu inicial  
- **MainGameScene** — Gameplay em Narandiba  
- **VictoryScene** — Tela de vitória  
- **GameOverScene** — Tela de derrota  

---

# 🧠 Game Design

**Invasão Zumbi em Narandiba** mistura:

- Ação  
- Combate corpo a corpo  
- Pressão constante das hordas  
- Evolução e temporização  
- Sobrevivência estratégica  

O ritmo é acelerado, dinâmico e centrado na tensão crescente das waves.

---

# 🎮 Elementos da Cena de Gameplay

- Player com movimentação física  
- Ataque corpo a corpo com hitbox  
- HUD com kills e timer  
- Zumbis lentos e rápidos  
- Sistema de waves  
- Evolução para Super Bárbaro  
- Câmera seguindo o player  

---

# 🧩 Scripts do Jogo

A arquitetura é modular, com cada script responsável por uma parte clara do jogo.

---

## 🔥 **PlayerController.cs**
Gerencia o jogador:
- Movimento com física  
- Ataque com hitbox  
- Vida, dano e morte  
- Evolução para **Super Bárbaro** (temporária)  
- HUD (kills + timer)  
- Detecção de vitória e derrota  
- Salvamento dos dados no `GameStats`  

---

## 🧟 **EnemyBasic.cs**
Lógica dos zumbis:
- Perseguem o player automaticamente  
- Param a uma distância segura e atacam com cooldown  
- Recebem dano e morrem  
- Avisam o spawner quando morrem  
- Somam kills ao player  

---

## 🌪️ **EnemySpawner.cs**
Sistema de waves:
- Configura waves com quantidades e frequências diferentes  
- Gera inimigos **ao redor do player**  
- Conta inimigos vivos  
- Avança as waves automaticamente  
- Quando tudo acaba → **Vitória**  

---

## 🗡️ **AttackHitbox.cs**
Sistema de ataque do player:
- Liga e desliga o collider durante o golpe  
- Detecta inimigos atingidos  
- Aplica dano com base no estado atual (normal ou super)  

---

## 📊 **GameStats.cs**
Script estático para armazenar:
- Total de kills  
- Resultado da partida (vitória ou derrota)  

---

## 🎥 **CameraFollow.cs**
Responsável por manter a câmera:
- Seguindo o jogador  
- Com movimento suave (Lerp)  
- Focada na ação principal  

---

## 🏁 **EndGameUI.cs**
Gerencia a interface das telas finais:
- Exibe vitória ou derrota  
- Mostra total de kills  
- Controla botões de reinício e menu  

---

## 🖥️ **MenuUI.cs**
Script dos botões do menu:
- Jogar  
- Reiniciar  
- Voltar ao menu  
- Sair  

---

# 📦 Conclusão

"Invasão Zumbi em Narandiba" entrega uma experiência de sobrevivência direta e intensa, com:
- Personagem evolutivo  
- Waves dinâmicas  
- Sistema de combate claro  
- Timer de sobrevivência  
- Cenas independentes para menu, jogo e finais  
