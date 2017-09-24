using tabuleiro;

namespace xadrex {
    class PosicaoXadrez {

        public char coluna { get; set; }
        public int  linha { get; set; }

        public PosicaoXadrez(char colua, int linha) {
            this.coluna = colua;
            this.linha = linha;
        }

        public Posicao toPosicao() {
            return new Posicao(8 - linha, coluna - 'A');
        }

        public override string ToString() {
            return "" + coluna + linha;
        }
    }
}
