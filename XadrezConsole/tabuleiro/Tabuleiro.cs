﻿namespace tabuleiro {
    class Tabuleiro {

        public int linhas { get; set; }
        public int colunas { get; set; }

        private Peca[,] pecas;

        public Tabuleiro(int linhas, int colunas) {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        public Peca peca (int linha, int coluna) {
            return pecas [linha, coluna];               //Retorna o objeto privado como público através do método criado.
        }

        public Peca peca (Posicao pos) {
            return pecas[pos.linha, pos.coluna];
        }

        public bool ExistePeca(Posicao pos) {
            ValidarPosicao(pos);
            return peca(pos) != null;
        }

        public void ColocarPeca(Peca p, Posicao pos) {
            if (ExistePeca(pos)) {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        public bool PosicaoValida(Posicao pos) {
            if (pos.linha < 0 || pos.linha >= linhas || pos.coluna < 0 || pos.coluna >= colunas) {
                return false; 
            }
            return true;
        }

        public void ValidarPosicao(Posicao pos) {
            if (!PosicaoValida(pos)) {
                throw new TabuleiroException("Posição Inválida!");

            }
        }


    }
}
