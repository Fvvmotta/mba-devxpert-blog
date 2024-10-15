# Feedback do Instrutor

#### 14/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Separação de responsabilidades.
- Arquitetura enxuta de acordo com a complexidade do projeto
- Bom controle de usuários com autorização e roles.
- Demonstrou conhecimento em Identity e JWT.
- Customizou a interface da aplicação com um novo layout
- Mostrou entendimento do ecossistema de desenvolvimento em .NET
- Documentou bem o repositório (faltou o Feedback.MD adicionei)
- Projeto pronto para rodar com SQLite

## Pontos Negativos:

- Não entendi a necessidade de criar um usuário e depois ter que criar um autor. O Autor é um usuário, pois mais que exista a entidade Autor e o registro de user (Identity) ambos são uma coisa só (mesmo em tabelas separadas).
- Faltou customizar a navegação do Identity de acordo com o novo layout, ficou meio confusa a navegação.
- Não encontrei caminho para criar um novo post com meu usuário.

## Sugestões:

- Unificar a criação do user + autor no mesmo processo. Utilize o ID do registro do Identity como o ID da PK do Autor, assim você mantém um link lógico entre os elementos.
