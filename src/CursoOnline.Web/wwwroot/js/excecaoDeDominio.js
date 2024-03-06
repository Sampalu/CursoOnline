function formQuandoFalha(erro) {
    if (erro.status == 500)
        alert(erro.responseJSON)
    else if (erro.status == 502)
        erro.responseJSON.forEach(function (mensagemDeErro) {
            alert(mensagemDeErro);
    });
}