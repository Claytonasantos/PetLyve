# Petlyfe - Projeto Petshop (CP1)

**Grupo:**

* Guilherme Sola Garcia - RM563674
* Clayton Alves dos Santos - RM562285

---

### Sobre o Projeto

A gente escolheu o domínio de um Petshop. O foco aqui foi modelar como funciona o atendimento, desde o dono chegando com o pet até o pagamento do serviço.

### O que foi modelado (Entidades):

* **Dono:** Dados de quem leva o pet.
* **Animal:** O pet que vai receber o serviço.
* **Funcionario:** Quem vai trabalhar no atendimento.
* **Servico:** O que vai ser feito (banho, tosa, etc) e o preço.
* **Agendamento:** Onde a gente junta o pet, o funcionário e o serviço com uma data.
* **Pagamento:** O registro do valor pago pelo agendamento.

### Relacionamentos:

* Um **Dono** pode ter vários **Animais** (1:N).
* Um **Animal** pode ter vários **Agendamentos** (1:N).
* Um **Funcionario** faz vários **Agendamentos** (1:N).
* Um **Servico** pode estar em vários **Agendamentos** (1:N).
* Um **Agendamento** gera um **Pagamento** (1:1).

### Estrutura do Código:

O projeto foi dividido seguindo a ideia de **Clean Arch**, com as pastas:

* **Domain**: Onde estão as nossas classes (entidades).
* **Application / Infrastructure / Api**: Criamos as pastas para deixar o projeto já estruturado.
