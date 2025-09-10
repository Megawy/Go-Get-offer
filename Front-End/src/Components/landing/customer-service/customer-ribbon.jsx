'use client'
import React from 'react'
import FlowingMenu from '../../common/floating-ribbon/flowting-ribbon'
import customer from '../../../../public/assets/customer-ribbon.svg'

import { useTranslation } from 'react-i18next';
import { useSearchParams } from 'next/navigation';


const CustomerRibbon = () => {
    const { t, i18n } = useTranslation(['customerRibbon']);
    const searchParams = useSearchParams();
    const lang = searchParams.get('lang') || i18n.language || 'en';
    const demoItems = [
        { link: '#customer', text: t('title'), image: customer, lang },
    ];
    return <>

        <section aria-labelledby='customer-service' id='#customer' style={{ height: '100px', position: 'relative' }}>
            <FlowingMenu items={demoItems} />
        </section>

    </>
}

export default CustomerRibbon