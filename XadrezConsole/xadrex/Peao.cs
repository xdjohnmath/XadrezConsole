using tabuleiro;

namespace xadrex {
    class Peao : Peca {

        private PartidaDeXadrez partida;

        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor) {
            this.partida = partida;
        }

        public override string ToString() {
            return "P";
        }

        private bool ExisteInimio(Posicao pos) {
            Peca p = tab.peca(pos);
            return p != null && p.cor != cor;
        }

        private bool livre(Posicao pos) {
            return tab.peca(pos) == null;
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);

            if (cor == Cor.Branca) {
                pos.DefinirValores(posicao.linha - 1, posicao.coluna);
                if (tab.PosicaoValida(pos) && livre(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha - 2, posicao.coluna);
                if (tab.PosicaoValida(pos) && livre(pos) && qteMovimentos == 0) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha - 1, posicao.coluna - 1);
                if (tab.PosicaoValida(pos) && ExisteInimio(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha - 1, posicao.coluna + 1);
                if (tab.PosicaoValida(pos) && ExisteInimio(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }

                // #jogadaespecial en passant

                if (posicao.linha == 3) {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if (tab.PosicaoValida(esquerda) && ExisteInimio(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant) {
                        mat[esquerda.linha-1, esquerda.coluna] = true;
                    }
                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.PosicaoValida(direita) && ExisteInimio(direita) && tab.peca(direita) == partida.vulneravelEnPassant) {
                        mat[direita.linha-1, direita.coluna] = true;
                    }

                }

            }
            else {
                pos.DefinirValores(posicao.linha + 1, posicao.coluna);
                if (tab.PosicaoValida(pos) && livre(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha + 2, posicao.coluna);
                if (tab.PosicaoValida(pos) && livre(pos) && qteMovimentos == 0) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha + 1, posicao.coluna - 1);
                if (tab.PosicaoValida(pos) && ExisteInimio(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.DefinirValores(posicao.linha + 1, posicao.coluna + 1);
                if (tab.PosicaoValida(pos) && ExisteInimio(pos)) {
                    mat[pos.linha, pos.coluna] = true;
                }

                // #jogadaespecial en passant

                if (posicao.linha == 4) {
                    Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
                    if (tab.PosicaoValida(esquerda) && ExisteInimio(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant) {
                        mat[esquerda.linha+1, esquerda.coluna] = true;
                    }
                    Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
                    if (tab.PosicaoValida(direita) && ExisteInimio(direita) && tab.peca(direita) == partida.vulneravelEnPassant) {
                        mat[direita.linha+1, direita.coluna] = true;
                    }

                }
            }

            return mat;
        }
    }
}
