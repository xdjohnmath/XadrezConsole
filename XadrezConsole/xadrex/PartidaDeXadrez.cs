using System;
using System.Collections.Generic;
using tabuleiro;

namespace xadrex {
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas = new HashSet<Peca>();
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            ColocarPecas();
            xeque = false;
            vulneravelEnPassant = null;
            capturadas = new HashSet<Peca>();

        }

        public Peca ExecutarMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQuantidadeDeMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if (pecaCapturada != null) {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQuantidadeDeMovimentos();
                tab.ColocarPeca(T, destinoT);
            }
            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQuantidadeDeMovimentos();
                tab.ColocarPeca(T, destinoT);
            }
            // #jogadaespecial en passant
            if(p is Peao) {
                if(origem.coluna != destino.coluna && pecaCapturada == null) {
                    Posicao posP;
                    if (p.cor == Cor.Branca) {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.RetirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void desfazOMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.RetirarPeca(destino);
            p.DecrementarQuantidadeDeMovimentos();

            if (pecaCapturada != null) {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQuantidadeDeMovimentos();
                tab.ColocarPeca(T, origemT);
            }
            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2) {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQuantidadeDeMovimentos();
                tab.ColocarPeca(T, origemT);
            }
            // #jogadaespecial en passant
            if(p is Peao) {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant) {
                    Peca peao = tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Branca) {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = ExecutarMovimento(origem, destino);

            Peca p = tab.peca(destino);

            if (EstaEmXeque(jogadorAtual)) {
                desfazOMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            // #jogadaespecial promoção
            if(p is Peao) {
                if(p.cor == Cor.Branca && destino.linha == 0|| p.cor == Cor.Preta && destino.linha == 7) {
                    p = tab.RetirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(tab, p.cor);
                    tab.ColocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }


            if (EstaEmXeque(adversaria(jogadorAtual))) {
                xeque = true;
            }
            else {
                xeque = false;
            }

            if (TesteXequeMate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                MudaJogador();
            }
            
            // #jogadaespecial en passant
            if(p is Peao&& (destino.linha == origem.linha-2 || destino.linha == origem.linha + 2)) {
                vulneravelEnPassant = p;
            }
            else {
                vulneravelEnPassant = null;
            }

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
            if (!tab.peca(origem).MovimentoPossivel(destino)) {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }

        private void MudaJogador() {
            if (jogadorAtual == Cor.Branca) {
                jogadorAtual = Cor.Preta;
            }
            else {
                jogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas) {
                if (x.cor == cor) {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor) {
            if (cor == Cor.Branca) {
                return Cor.Preta;
            }
            else {
                return Cor.Branca;
            }
        }

        private Peca rei(Cor cor) {
            foreach (Peca x in PecasEmJogo(cor)) {
                if (x is Rei) {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor) {
            Peca R = rei(cor);
            if (R == null) {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro.");
            }

            foreach (Peca x in PecasEmJogo(adversaria(cor))) {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor) {
            if (!EstaEmXeque(cor)) {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor)) {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0 ; i < tab.linhas ; i++) {
                    for (int j = 0 ; j < tab.colunas ; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            desfazOMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas() {
            //BRANCAS
            ColocarNovaPeca('A', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('B', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('C', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('D', 1, new Dama(tab, Cor.Branca));
            ColocarNovaPeca('E', 1, new Rei(tab, Cor.Branca, this));
            ColocarNovaPeca('F', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('G', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('H', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('A', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('B', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('C', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('D', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('E', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('F', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('G', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('H', 2, new Peao(tab, Cor.Branca, this));

            //PRETAS

            ColocarNovaPeca('A', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('B', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('C', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('D', 8, new Dama(tab, Cor.Preta));
            ColocarNovaPeca('E', 8, new Rei(tab, Cor.Preta, this));
            ColocarNovaPeca('F', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('G', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('H', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('A', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('B', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('C', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('D', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('E', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('F', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('G', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('H', 7, new Peao(tab, Cor.Preta, this));
        }
    }
}
