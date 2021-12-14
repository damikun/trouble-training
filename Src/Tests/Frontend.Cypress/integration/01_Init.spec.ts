export{}

describe('Reset Database', () => {

  it('Reset Database', () => {
    cy.resetdb()
  });

});


describe('User Login', () => {

  it('Login', () => {
    cy.login("testuser","testuser")
  });

});
