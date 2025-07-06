document.addEventListener('DOMContentLoaded', () => {
    // Client Dropdown + Address Mapping 
    const addressSelect = document.querySelector('select[name="address_id"]');
    // Client (Search + Tag, one only)
    const clientSearch   = document.getElementById('client-search');
    const clientAddBtn   = document.getElementById('client-add-btn');
    const clientTagWrap  = document.getElementById('client-tag');
    const clientDataList = document.getElementById('client-list');



    function setClient(name, id) {
        clientTagWrap.innerHTML = ''; // Only one tag
        const tag = document.createElement('div');
        tag.className = 'client-chip flex items-center bg-purple-100 text-purple-800 px-3 py-1 rounded-full';
        tag.dataset.id = id;
        tag.innerHTML = `
            ${name}
            <button type="button" class="ml-2 text-xl leading-none remove-client">&times;</button>
            <input type="hidden" name="client_id" value="${id}" data-id="${id}">
        `;
        clientTagWrap.appendChild(tag);
    }

    clientAddBtn?.addEventListener('click', () => {
        const name = clientSearch.value.trim();
        if (!name) return;
        const option = Array.from(clientDataList.options).find(o => o.value === name);
        if (!option) return;
    
        const clientId = option.dataset.id;
        setClient(name, clientId);
        clientSearch.value = '';
    
        const addresses = window.addressMap?.[clientId] || [];
        addressSelect.innerHTML = '<option value="">— Select Address —</option>';
        addresses.forEach(function (addr) {
            const option = document.createElement('option');
            option.value = addr.id;                     
            option.textContent = addr.label;            
            addressSelect.appendChild(option);
        });
        
    });
    

    clientSearch?.addEventListener('keydown', e => {
        if (e.key === 'Enter') {
            e.preventDefault();
            clientAddBtn.click();
        }
    });

    clientTagWrap?.addEventListener('click', e => {
        if (!e.target.classList.contains('remove-client')) return;
        clientTagWrap.innerHTML = ''; // Remove client tag
    });

    //  Technicians (Multiple Tags) 
    const techSearch   = document.getElementById('tech-search');
    const techAddBtn   = document.getElementById('tech-add-btn');
    const techTagWrap  = document.getElementById('tech-tags');
    const techDataList = document.getElementById('tech-list');

    function addTech(name, id) {
        if (techTagWrap.querySelector(`.tech-tag[data-id="${id}"]`)) return;
        const tag = document.createElement('div');
        tag.className = 'tech-tag flex items-center bg-blue-100 text-blue-800 px-3 py-1 rounded-full';
        tag.dataset.id = id;
        tag.innerHTML = `
            ${name}
            <button type="button" class="ml-2 text-xl leading-none remove-tech">&times;</button>
            <input type="hidden" name="technician_ids[]" value="${id}" data-id="${id}">
        `;
        techTagWrap.appendChild(tag);
    }

    techAddBtn?.addEventListener('click', () => {
        const name = techSearch.value.trim();
        if (!name) return;
        const option = Array.from(techDataList.options).find(o => o.value === name);
        if (!option) return;
        addTech(name, option.dataset.id);
        techSearch.value = '';
    });

    techSearch?.addEventListener('keydown', e => {
        if (e.key === 'Enter') {
            e.preventDefault();
            techAddBtn.click();
        }
    });

    techTagWrap?.addEventListener('click', e => {
        if (!e.target.classList.contains('remove-tech')) return;
        e.target.closest('.tech-tag')?.remove();
    });

    // ----------- Supervisor (Single Tag) ----------------------
    const supSearch   = document.getElementById('supervisor-search');
    const supAddBtn   = document.getElementById('supervisor-add-btn');
    const supTagWrap  = document.getElementById('supervisor-tag');
    const supDataList = document.getElementById('supervisor-list');

    function setSupervisor(name, id) {
        supTagWrap.innerHTML = '';
        const tag = document.createElement('div');
        tag.className = 'supervisor-chip flex items-center bg-green-100 text-green-800 px-3 py-1 rounded-full';
        tag.dataset.id = id;
        tag.innerHTML = `
            ${name}
            <button type="button" class="ml-2 text-xl leading-none remove-supervisor">&times;</button>
            <input type="hidden" name="supervisor_id" value="${id}" data-id="${id}">
        `;
        supTagWrap.appendChild(tag);
    }

    supAddBtn?.addEventListener('click', () => {
        const name = supSearch.value.trim();
        if (!name) return;
        const option = Array.from(supDataList.options).find(o => o.value === name);
        if (!option) return;
        setSupervisor(name, option.dataset.id);
        supSearch.value = '';
    });

    supSearch?.addEventListener('keydown', e => {
        if (e.key === 'Enter') {
            e.preventDefault();
            supAddBtn.click();
        }
    });

    supTagWrap?.addEventListener('click', e => {
        if (!e.target.classList.contains('remove-supervisor')) return;
        supTagWrap.innerHTML = '';
    });
});
