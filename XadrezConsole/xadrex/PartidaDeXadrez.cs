using System;
using tabuleiro;

namespace xadrex{
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            ColocarPecas();
            
        }

        public void ExecutarMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQuantidadeDeMovimentos();
            Peca pecaCapturaa = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);

        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            ExecutarMovimento(origem, destino);
            turno++;
            MudaJogador();
        }

        public void validaPosicaoDeOrigem(Posicao pos) {
            if (tab.peca(pos) == null) {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor) {
                throw new TabuleiroException("A peça escolhida não é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis()) {
                throw new TabuleiroException("Não há movimentos possível para a peça escolhida");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino) {
            if (!tab.peca(origem).PodeMoverPara(destino)) {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }

        private void MudaJogador() {
            if(jogadorAtual == Cor.Branca) {
                jogadorAtual = Cor.Preta;
            }
            else {
                jogadorAtual = Cor.Branca;
            }
        }

        private void ColocarPecas() {

            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('C',1).toPosicao());

            tab.ColocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('D', 1).toPosicao());


        }
    }
}
