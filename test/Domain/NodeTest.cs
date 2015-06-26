using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AG.Framework.Utils;

namespace AG.Framework.Domain
{
    [TestFixture]
    public class NodeTest
    {
        protected List<ClasseTeste> ArvoreUmRootNode { get; set; }
        protected List<ClasseTeste> ArvoreDoisRootNodes { get; set; }

        [SetUp]
        public void Initialize()
        {
            #region Árvore com somente um root node

            ArvoreUmRootNode = new List<ClasseTeste>
                           {
                               new ClasseTeste
                                   {
                                       Id = 1,
                                       IdPai = null
                                   }
                               ,
                               new ClasseTeste
                                   {
                                       Id = 2,
                                       IdPai = 1
                                   }
                           };
            #endregion


            #region Árvore com dois root nodes

            ArvoreDoisRootNodes = new List<ClasseTeste>
                           {
                               new ClasseTeste
                                   {
                                       Id = 1,
                                       IdPai = null
                                   }
                               ,
                               new ClasseTeste
                                   {
                                       Id = 100,
                                       IdPai = null
                                   }
                               ,
                               new ClasseTeste
                                   {
                                       Id = 2,
                                       IdPai = 1
                                   }
                                   ,
                               new ClasseTeste
                                   {
                                       Id = 3,
                                       IdPai = 1
                                   }
                                   ,
                               new ClasseTeste
                                   {
                                       Id = 4,
                                       IdPai = 1
                                   }
                                   ,
                               new ClasseTeste
                                   {
                                       Id = 5,
                                       IdPai = 100
                                   }
                           };
            #endregion
        }

        [Test]
        public void TestaMontagemDaArvore()
        {   
            var arvore = ArvoreUmRootNode.ByHierarchyNode(x => x.IdPai == null, (parent, child) => parent.Id == child.IdPai);
            Assert.That(arvore, Is.Not.Null);
        }
            
        [Test]
        public void QuantidadeDeFilhos()
        {
            var arvore = ArvoreUmRootNode.ByHierarchyNode(x => x.IdPai == null, (parent, child) => parent.Id == child.IdPai);
            Assert.That(arvore, Is.Not.Null);
            Assert.That(arvore.ToList().Count, Is.EqualTo(1));
            Assert.That(arvore.Select(node => node.Children).ToList().Count, Is.EqualTo(1));
        }

        [Test]
        public void MaisDeUmRoot()
        {
            var arvore = ArvoreDoisRootNodes.ByHierarchyNode(x => x.IdPai == null, (parent, child) => parent.Id == child.IdPai);
            Assert.That(arvore, Is.Not.Null);
            Assert.That(arvore.ToList().Count, Is.EqualTo(2));
        }

        [Test]
        public void MaisDeUmFilho()
        {
            var arvore = ArvoreDoisRootNodes.ByHierarchyNode(x => x.IdPai == null, (parent, child) => parent.Id == child.IdPai);
            Assert.That(arvore, Is.Not.Null);
            Assert.That(arvore.ToList().Count, Is.EqualTo(2));
            Assert.That(arvore.ToList()[0].Children.Count, Is.EqualTo(3));
            Assert.That(arvore.ToList()[1].Children.Count, Is.EqualTo(1));
        }
    }

    public class ClasseTeste: Node
    {        
    }
}
