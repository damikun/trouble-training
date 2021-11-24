
export{}


describe('Logout Tests', () => {

  beforeEach(()=>{
    cy.login("testuser","testuser")
  })
  
  //--------------------------------------
  //--------------------------------------
  
  it('Logout using Menu', () => {

    cy.get('#__UserMenuDropdown')
    .click()
    .contains('Logout')
    .click()

    // cy.get('button')
    // .should('have.value', "logout")
    // .click()

    cy.contains("You are now logged out")
    
  });

});
