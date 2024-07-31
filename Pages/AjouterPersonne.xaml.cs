using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Test_api.Service;
using WPFModernVerticalMenu.Model;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Logique d'interaction pour AjouterPersonne.xaml
    /// </summary>
    public partial class AjouterPersonne : Page

    {
        private readonly PersonneService _personneService;
        public AjouterPersonne()
        {
            _personneService = new PersonneService();
            InitializeComponent();
            ChargerPersonnes();
        }
        private async Task ChargerPersonnes()
        {
            var personnes = await _personneService.ObtenirToutesLesPersonnes();
            dgPersonne.ItemsSource = personnes;
        }
        /// <summary>
        /// Cette méthode permet d'ajouter une personne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAjouter_click(object sender, RoutedEventArgs e)
        {
            Personne personne = new Personne
            {
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                Age = int.Parse(txtAge.Text)
            };

            bool success = await _personneService.AjouterPersonne(personne);
            if (success)
            {
                MessageBox.Show("Personne ajoutée avec succès !");
                await ChargerPersonnes();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout de la personne.");
            }
            resetForm();
        }
        public void resetForm()
        {
            txtNom.Text = "";
            txtAge.Text = "";
            txtPrenom.Text = "";

        }

        private void dgPersonne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// Cette méthode permet de modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnModifier_click(object sender, RoutedEventArgs e)
        {
            if (dgPersonne.SelectedItem is Personne selectedPersonne)
            {
                selectedPersonne.Nom = txtNom.Text;
                selectedPersonne.Prenom = txtPrenom.Text;
                selectedPersonne.Age = int.Parse(txtAge.Text);

                bool result = await _personneService.MettreAJourPersonne(selectedPersonne);
                if (result)
                {
                    MessageBox.Show("Utilisateur modifié avec succès !");
                    ChargerPersonnes();
                    resetForm();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de l'utilisateur.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à modifier.");
            }
        
    }
        /// <summary>
        /// Cette méthode permet de selectionner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectionner_click(object sender, RoutedEventArgs e)
        {
            if (dgPersonne.SelectedItem is Personne selectedPersonne)
            {

                txtNom.Text = selectedPersonne.Nom;
                txtPrenom.Text = selectedPersonne.Prenom;
                txtAge.Text = selectedPersonne.Age.ToString();
                btnAjouter.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à éditer.");
            }
        }
        /// <summary>
        /// Cette méthode permette de supprimer une personne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSupprimer_click(object sender, RoutedEventArgs e)
        {
            if (dgPersonne.SelectedItem is Personne selectedPersonne)
            {
                bool result = await _personneService.SupprimerPersonne(selectedPersonne.Id);
                if (result)
                {
                    MessageBox.Show("Utilisateur supprimé avec succès !");
                    ChargerPersonnes();
                    resetForm();
                    btnAjouter.IsEnabled=false;
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression de l'utilisateur.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer.");
            }
        }
    }
}
