﻿using System;
using tabuleiro;

namespace xadrex{
    class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;

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

        private void ColocarPecas() {

            tab.ColocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c',1).toPosicao());

            tab.ColocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('d', 1).toPosicao());


        }
    }
}