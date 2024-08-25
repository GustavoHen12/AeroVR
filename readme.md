# AeroVR
Jogo em realidade virtual de treinamento para compensação de perda do campo visual para pacientes pós-avc

## Descrição
Anualmente, o Acidente Vascular Cerebral (AVC) afeta milhares de pessoas, sendo uma das principais causas de óbitos no Brasil. Entre os sobreviventes, muitos enfrentam sequelas permanentes, como a perda parcial do campo de visão, conhecida como hemianopsia. A estratégia de tratamento mais comum para essa condição, consistem em, por meio de diferentes execícios, estimular o paciente a compensar de alguma forma essa perca do campo visual. Entretanto, assim como a maioria dos métodos de terapias tradicionais, esses exercícios podem ser pouco motivadores para os pacientes. Compreendendo os desafios da reabilitação voltada à compensação dessa perda visual, desenvolvemos o AeroVR, um jogo projetado para auxiliar na recuperação dos pacientes. O jogo integra dois conceitos fundamentais: a realidade virtual, que permite estimular uma ampla área do campo visual, e os jogos sérios, que promovem maior engajamento dos pacientes, incentivando o tratamento. Todo o projeto foi desenvolvido de forma iterativa em colaboração com a Clínica-Escola de Terapia Ocupacional da UFPR.

## Jogo
O jogo pode ser utilizado em três modos: standalone, ou seja, apenas no smartphone, espelhando a tela do smartphone em uma tela com o auxílio de um computador e em juntamente com um óculos de realidade virtual.


### Instalando o aplicativo
O AeroVR está disponível apenas para dispositivos Android, sendo que, o processo de instalação é muito similar a um aplicativo tradicional.

Acesse o link do repositório [link] e faça o download da versão mais recente do aplicativo. O aplicativo é o arquivo `aero_vr.apk` disponível para download. É possível realizar o download por meio de um computador e posteriormente transferir para o smartphone desejado, ou realizar diretamente no smartphone.

Após realizar o download, realize a instalação do aplicativo no dispositivo móvel desejado. Para isso, através do gerenciador de arquivos do dispositivo, encontre o arquivo `.apk` e abra o arquivo. Com isso, será iniciado o processo de instalação do aplicativo. Dependendo das configurações do seu dispositivo pode ser necessário permitir a instalação de aplicativos de fontes externas, para isso siga as instruções que serão exibidas no dispositivo e após isso realize a instalação normalmente.

### Configurando
Após instalado o aplicativo, a primeira coisa a ser realizada é a configuração de um paciente para poder iniciar a partida. Para isso basta:
1. No menu inicial clique na opção "Selecionar Paciente"
2. Na caixa de texto superior escreva o nome do paciente inicial e pressione "Adicionar"
3. No menu de seleção abaixo na tela, selecione o nome do paciente adicionado
4. Volte para o menu principal
5. Clique em configurações
6. Realize a configuração dos parâmetros para o paciente. **Nesse momento deve ser configurado a disposição das placas, pois a configuração padrão não exibe nenhuma placa**
7. Salve as configurações

Após esses passos, o jogo estará configurado para ser utilizado no dispositivo móvel desejado e basta iniciar a partida.

### Modos de uso
O jogo possui 3 diferentes modos de uso, aqui falaremos das particularidades de cada uma:

#### Standalone
Nesse caso o jogo será exibido no próprio smartphone, para controlar o avião pode ser utilizado a própria tela do smartphone, basta deslizar o dedo para esquerda ou direita. Ressalto que esse é o método que menos estímulo que o jogo seja utilizado, uma vez que a área do campo visual preenchido pela tela do smartphone é muito limitada, portanto o uso terá pouco ou nenhum benefício para o paciente.

#### Monitor externo
Para o uso com o monitor externo, basta espelhar a tela do dispositivo móvel no monitor. Para isso recomendamos o uso da aplicação para computador [scrcpy](https://github.com/Genymobile/scrcpy). Esse aplicativo permite espelhar a tela do smartphone no computado e está disponível para os principais sistemas operacionais (Linux, macOS e Windows). Para utilizar essa ferramenta, basta acessar o [link do repositório](https://github.com/Genymobile/scrcpy) e seguir as instruções de instalação disponíveis.

Um dos grandes benefícios dessa ferramenta é que ela permite que o mouse e o teclado do computador interajam com o smartphone. Portanto, nesse modo temos duas possibilidades de controle: através do mouse do computador ou utilizando as setas do teclado. 

#### Óculos VR
Para utilizar o jogo no modo VR é necessário ter em mente dois pontos importantes:
- O smartphone precisa possuir os sensores necessários para o uso em realidade virtual. Para conferir isso basta verificar os sensores disponíveis no smartphone ou tente reproduzir um vídeo em realidade virtual, movimente o celular e veja se o vídeo acompanha o movimento.
- Devemos ter um óculos VR compatível com o celular. De modo geral todo óculos VR no estilo do Google Cardboard, ou seja, em que o óculos possui apenas as lentes e serve como um adaptador para o smartphone deve funcionar corretamente. Para o desenvolvimento desse projeto foi utilizado o modelo [VR Shinecon](https://shopee.com.br/%C3%93culos-Vr-Realidade-Virtual-3d-Com-Fone-De-Ouvido-E-Controle-i.493663790.17919996467?sp_atk=35e2afe4-68d9-4add-8ffa-3320427605c6&xptdk=35e2afe4-68d9-4add-8ffa-3320427605c6) 
- Um controle bluetooth para ser utilizado durante o jogo. Apesar do controle bluetooth para óculos VR ser o mais simples, ressaltamos que qualquer mouse bluetooth conectado ao smartphone irá funcionar corretamente.
 
 Com todos esses equipamentos corretamente verificados e disponíveis, basta habilitar o modo VR nas configurações e iniciar a partida. Após isso, insira o smartphone no óculos VR e jogue normalmente.
